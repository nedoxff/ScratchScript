using System.Reflection;
using Antlr4.Runtime;
using ScratchScript.Core.Diagnostics;
using ScratchScript.Helpers;
using Serilog;

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor
{
    public Dictionary<string, (string, ParserRuleContext)> Imports = new();
    public Dictionary<string, (ParserRuleContext, List<DefinedScratchFunction>)> FunctionNamespaces = new();
    public List<DefinedScratchFunction> ImportedFunctions = new();

    public override TypedValue? VisitImportStatement(ScratchScriptParser.ImportStatementContext context)
    {
        var name = context.String().GetText()[1..^1];
        var what = context.LeftBrace() == null ? null : context.Identifier().Select(i => i.GetText().Trim());
        var to = context.importAll() == null || context.importAll().Identifier() == null
            ? null
            : context.importAll().Identifier().GetText();
        ImportInternal(name, what, to, context);
        return null;
    }

    public void ImportInternal(string name, IEnumerable<string> what = null, string to = null,
        ScratchScriptParser.ImportStatementContext context = null)
    {
        if (name.StartsWith("std"))
        {
            var filename = name.TrimEnd('/') + ".scrs";
            var path = Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), filename);
            if (!File.Exists(path))
            {
                if (context != null)
                    DiagnosticReporter.Error(ScratchScriptError.UnknownNamespace, context, context.String().Symbol,
                        name);
                return;
            }

            Log.Information("Importing a standard file ({File})", filename);

            var inputStream = new AntlrFileStream(path);
            var lexer = new ScratchScriptLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new ScratchScriptParser(tokenStream);
            var visitor = new ScratchScriptVisitor(parser, path);

            visitor.Visit(parser.program());
            if (!visitor.Success)
            {
                if (context != null)
                    DiagnosticReporter.Error(ScratchScriptError.IceStdFileContainsErrors, context,
                        context.String().Symbol);
                return;
            }

            var functions = visitor.Functions.OfType<DefinedScratchFunction>().ToList();
            if (what == null)
            {
                ImportedFunctions.AddRange(functions);
                if(!string.IsNullOrEmpty(to)) FunctionNamespaces.Add(to, (context, functions));
            }
            else
            {
                var importList = what.ToList();
                if(!string.IsNullOrEmpty(to)) FunctionNamespaces.Add(to, (context, new()));
                for (var idx = 0; idx < importList.Count; idx++)
                {
                    var function = importList[idx];
                    if (functions.All(f => f.BlockInformation.Name != function))
                    {
                        if (context != null)
                            DiagnosticReporter.Error(ScratchScriptError.FunctionDoesNotExistInImportedFile, context,
                                context.Identifier(idx).Symbol, function);
                        return;
                    }

                    var import = functions.First(f => f.BlockInformation.Name == function);
                    ImportedFunctions.Add(import);
                    if(!string.IsNullOrEmpty(to)) FunctionNamespaces[to].Item2.Add(import);
                }
            }

            ImportedProceduresSection += visitor.ImportedProceduresSection;
            //Functions = Functions.Union(functions).DistinctBy(f => f.BlockInformation.Name).ToList();
            ImportedFunctions.AddRange(visitor.ImportedFunctions);
            Imports = Imports.Concat(visitor.Imports).DistinctBy(i => i.Key)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            Imports.Add(name.TrimEnd('/'), (Path.GetFileName(InputFile), context));
            Log.Debug("Imported {Count} functions ({Namespace})", functions.Count, visitor.Namespace);
            return;
        }

        //TODO: add import from other files

        if (context != null)
            DiagnosticReporter.Error(ScratchScriptError.UnknownNamespace, context, context.String().Symbol, name);
    }

    public void RequireFunction(string name, ParserRuleContext context)
    {
        var importedFunction = Functions.FirstOrDefault(f => f.BlockInformation.Name == name);
        if (importedFunction != null)
        {
            if (Scope.IsInsideFunction() && importedFunction is DefinedScratchFunction definedFunction)
            {
                var procedure = Procedures.Last();
                procedure.Dependencies.AddRange(new[] { definedFunction.BlockInformation.Name }
                    .Concat(definedFunction.Dependencies));
                procedure.Dependencies = procedure.Dependencies.Distinct().ToList();
            }

            return;
        }


        if (ImportedFunctions.All(f => f.BlockInformation.Name != name))
        {
            DiagnosticReporter.Error(ScratchScriptError.IceImportedFunctionIsNotDefined, context, context, name);
            return;
        }

        var function = ImportedFunctions.First(f => f.BlockInformation.Name == name);
        foreach (var dependency in function.Dependencies)
            RequireFunction(dependency, context);
        Functions.Add(function);
        ImportedProceduresSection += function.Code + "\n\n";
        Log.Debug("Copied imported function {Name} from {Namespace}", name, function.BlockInformation.Namespace);
    }
}