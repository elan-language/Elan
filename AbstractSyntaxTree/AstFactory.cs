using System.Collections.Immutable;
using AbstractSyntaxTree.Nodes;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using static ElanParser;

namespace AbstractSyntaxTree;

public static class AstFactory {
    private static T Visit<T>(this ElanBaseVisitor<IAstNode> visitor, IParseTree pt) where T : IAstNode => (T)visitor.Visit(pt);

    public static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ParserRuleContext context) =>
        context switch {
            FileContext c => visitor.Build(c),
            MainContext c => visitor.Build(c),
            StatementBlockContext c => visitor.Build(c),
            CallStatementContext c => visitor.Build(c),
            ExpressionContext c => visitor.Build(c),
            MethodCallContext c => visitor.Build(c),
            ValueContext c => visitor.Build(c),
            LiteralValueContext c => visitor.Build(c),
            LiteralKvpContext c => visitor.Build(c),
            LiteralDataStructureContext c => visitor.Build(c),
            VarDefContext c => visitor.Build(c),
            ConstantDefContext c => visitor.Build(c),
            AssignableValueContext c => visitor.Build(c),
            AssignmentContext c => visitor.Build(c),
            InlineAsignmentContext c => visitor.Build(c),
            LiteralContext c => visitor.Build(c),
            BinaryOpContext c => visitor.Build(c),
            ArithmeticOpContext c => visitor.Build(c),
            LogicalOpContext c => visitor.Build(c),
            ProceduralControlFlowContext c => visitor.Build(c),
            IfContext c => visitor.Build(c),
            ForContext c => visitor.Build(c),
            ForeachContext c => visitor.Build(c),
            WhileContext c => visitor.Build(c),
            RepeatContext c => visitor.Build(c),
            SwitchContext c => visitor.Build(c),
            ConditionalOpContext c => visitor.Build(c),
            UnaryOpContext c => visitor.Build(c),
            LiteralListContext c => visitor.Build(c),
            LiteralTupleContext c => visitor.Build(c),
            LiteralDictionaryContext c => visitor.Build(c),
            IndexContext c => visitor.Build(c),
            RangeContext c => visitor.Build(c),
            NewInstanceContext c => visitor.Build(c),
            TypeContext c => visitor.Build(c),
            DataStructureTypeContext c => visitor.Build(c),
            DataStructureDefinitionContext c => visitor.Build(c),
            ArrayDefinitionContext c => visitor.Build(c),
            ListDefinitionContext c => visitor.Build(c),
            GenericSpecifierContext c => visitor.Build(c),
            ProcedureDefContext c => visitor.Build(c),
            FunctionDefContext c => visitor.Build(c),
            SystemAccessorContext c => visitor.Build(c),
            FunctionWithBodyContext c => visitor.Build(c),
            ProcedureSignatureContext c => visitor.Build(c),
            FunctionSignatureContext c => visitor.Build(c),
            AccessorSignatureContext c => visitor.Build(c),
            ParameterContext c => visitor.Build(c),
            CaseContext c => visitor.Build(c),
            CaseDefaultContext c => visitor.Build(c),
            TryContext c => visitor.Build(c),
            EnumDefContext c => visitor.Build(c),
            EnumTypeContext c => visitor.Build(c),
            EnumValueContext c => visitor.Build(c),
            ClassDefContext c => visitor.Build(c),
            ImmutableClassContext c => visitor.Build(c),
            MutableClassContext c => visitor.Build(c),
            AbstractClassContext c => visitor.Build(c),
            AbstractImmutableClassContext c => visitor.Build(c),
            ConstructorContext c => visitor.Build(c),
            PropertyContext c => visitor.Build(c),
            ScopeQualifierContext c => visitor.Build(c),
            DeconstructedTupleContext c => visitor.Build(c),
            ThrowExceptionContext c => visitor.Build(c),
            PrintStatementContext c => visitor.Build(c),

            _ => throw new NotImplementedException(context?.GetType().FullName ?? null)
        };

    public static IAstNode BuildTerminal(this ElanBaseVisitor<IAstNode> visitor, ITerminalNode node) => new IdentifierNode(node.Symbol.Text);

    private static FileNode Build(this ElanBaseVisitor<IAstNode> visitor, FileContext context) {
        var constants = context.constantDef().Select(visitor.Visit);
        var procedures = context.procedureDef().Select(visitor.Visit);
        var functions = context.functionDef().Select(visitor.Visit);
        var systemAccessors = context.systemAccessor().Select(visitor.Visit);
        var enums = context.enumDef().Select(visitor.Visit);
        var classes = context.classDef().Select(visitor.Visit);
        var globals = constants.Concat(procedures).Concat(functions).Concat(systemAccessors).Concat(enums).Concat(classes).ToImmutableArray();
        var mainNode = context.main().Select(visitor.Visit<MainNode>).SingleOrDefault();

        return new FileNode(globals, mainNode);
    }

    private static StatementBlockNode Build(this ElanBaseVisitor<IAstNode> visitor, StatementBlockContext context) {
        var statements = context.children is { } c ? c.Select(visitor.Visit) : Array.Empty<IAstNode>();
        return new StatementBlockNode(statements.ToImmutableArray());
    }

    private static MainNode Build(this ElanBaseVisitor<IAstNode> visitor, MainContext context) {
        var block = visitor.Visit<StatementBlockNode>(context.statementBlock());

        return new MainNode(block);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, CallStatementContext context) {
        if (context.DOT() is not null) {
            var ms = visitor.Visit<MethodCallNode>(context.methodCall());
            var exp = visitor.Visit(context.assignableValue());

            return  new CallStatementNode(ms with { DotCalled = true, Qualifier = exp });
        }

        return  new CallStatementNode(visitor.Visit(context.methodCall()));
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ExpressionContext context) {
        if (context.DOT() is not null) {
            if (context.methodCall() is { } dmc) {
                var ms = visitor.Visit<MethodCallNode>(dmc);
                var exp = visitor.Visit(context.expression().First());
                //return new MethodCallNode(ms, exp) { DotCalled = true };

                return ms with { DotCalled = true, Qualifier = exp };
            }

            if (context.IDENTIFIER() is { } id) {
                var exp = visitor.Visit(context.expression().First());
                var prop = visitor.Visit(id);
                return new PropertyNode(exp, prop);
            }
        }

        if (context.methodCall() is { } mc) {
            return visitor.Visit(mc);
        }

        if (context.binaryOp() is { } bop) {
            return new BinaryNode(visitor.Visit(bop), visitor.Visit(context.expression().First()), visitor.Visit(context.expression().Last()));
        }

        if (context.POWER() is { } p) {
            var op = new OperatorNode(Helpers.MapOperator(p.Symbol.Type));
            return new BinaryNode(op, visitor.Visit(context.expression().First()), visitor.Visit(context.expression().Last()));
        }

        if (context.value() is { } v) {
            return visitor.Visit(v);
        }

        if (context.bracketedExpression() is { } b) {
            return new BracketNode(visitor.Visit(b.expression()));
        }

        if (context.unaryOp() is { } uop) {
            return new UnaryNode(visitor.Visit(uop), visitor.Visit(context.expression().Single()));
        }

        if (context.index() is { } idx) {
            var expr = visitor.Visit(context.expression().Single());
            var range = visitor.Visit(idx);

            return new IndexedExpressionNode(expr, range);
        }

        if (context.newInstance() is { } ni) {
            return visitor.Visit(ni);
        }

        if (context.NL() is not null) {
            return visitor.Visit(context.expression().Single());
        }

        if (context.withClause() is { } wc) {
            var expr = visitor.Visit(context.expression().Single());
            var with = wc.inlineAsignment().Select(visitor.Visit);

            return new WithNode(expr, with.ToImmutableArray());
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, MethodCallContext context) {
        var id = visitor.Visit(context.IDENTIFIER());
        var pps = (context.argumentList()?.expression().Select(visitor.Visit) ?? Array.Empty<IAstNode>()).ToImmutableArray();

        var nqr = context.scopeQualifier() is { } nq ? visitor.Visit(nq) : null;

        return new MethodCallNode(id, nqr, pps);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ValueContext context) {
        if (context.scopeQualifier() is { } nq) {
            var nqn = visitor.Visit(nq);
            var idn = visitor.Visit(context.IDENTIFIER());
            return new QualifiedNode(nqn, idn);
        }

        if (context.literal() is { } lv) {
            return visitor.Visit(context.literal());
        }

        if (context.IDENTIFIER() is { } id) {
            return visitor.Visit(id);
        }

        if (context.dataStructureDefinition() is { } ds) {
            return visitor.Visit(ds);
        }

        if (context.SELF() is { } s) {
            return new SelfNode();
        }

        if (context.DEFAULT() is not null) {
            return new DefaultNode(visitor.Visit(context.type()));
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, VarDefContext context) {
        var id = visitor.Visit(context.assignableValue());
        var expr = visitor.Visit(context.expression());

        return new VarDefNode(id, expr);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ConstantDefContext context) {
        var id = visitor.Visit(context.IDENTIFIER());
        var expr = context.literal() is { } l ? visitor.Visit(l) : null;
        var newInstance = context.newInstance() is { } n ? visitor.Visit(n) : null;

        return new ConstantDefNode(id, expr ?? newInstance!);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, AssignmentContext context) {
        var id = visitor.Visit(context.assignableValue());
        var expr = visitor.Visit(context.expression());

        return new AssignmentNode(id, expr, false);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, InlineAsignmentContext context) {
        var id = visitor.Visit(context.assignableValue());
        var expr = visitor.Visit(context.expression());

        return new AssignmentNode(id, expr, true);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, AssignableValueContext context) {
        if (context.index() is { } idx) {
            var index = visitor.Visit(idx);
            var expr = visitor.Visit(context.IDENTIFIER());

            return new IndexedExpressionNode(expr, index);
        }

        if (context.scopeQualifier() is { } nq) {
            var qual = visitor.Visit(nq);
            var expr = visitor.Visit(context.IDENTIFIER());

            return new QualifiedNode(qual, expr);
        }

        if (context.IDENTIFIER() is { } id) {
            return visitor.Visit(id);
        }

        if (context.deconstructedTuple() is { } dt) {
            return visitor.Visit(dt);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, LiteralValueContext context) {
        if (context.LITERAL_INTEGER() is { } i) {
            return new ValueNode(i.Symbol.Text, new ValueTypeNode(ValueType.Int));
        }

        if (context.LITERAL_FLOAT() is { } f) {
            return new ValueNode(f.Symbol.Text, new ValueTypeNode(ValueType.Float));
        }

        if (context.LITERAL_CHAR() is { } c) {
            return new ValueNode(c.Symbol.Text, new ValueTypeNode(ValueType.Char));
        }

        if (context.BOOL_VALUE() is { } b) {
            return new ValueNode(b.Symbol.Text, new ValueTypeNode(ValueType.Bool));
        }

        if (context.enumValue() is { } ev) {
            return visitor.Visit(ev);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, LiteralKvpContext context) {
        var key = visitor.Visit(context.literal().First());
        var value = visitor.Visit(context.literal().Last());

        return new PairNode(key, value);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, LiteralDataStructureContext context) {
        if (context.LITERAL_STRING() is { } ls) {
            return new ValueNode(ls.Symbol.Text, new ValueTypeNode(ValueType.String));
        }

        if (context.literalList() is { } ll) {
            return visitor.Visit(ll);
        }

        if (context.literalDictionary() is { } ld) {
            return visitor.Visit(ld);
        }

        if (context.literalTuple() is { } lt) {
            return visitor.Visit(lt);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, LiteralContext context) {
        if (context.literalValue() is { } lv) {
            return visitor.Visit(lv);
        }

        if (context.literalDataStructure() is { } lds) {
            return visitor.Visit(lds);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, BinaryOpContext context) {
        if (context.arithmeticOp() is { } ao) {
            return visitor.Visit(ao);
        }

        if (context.conditionalOp() is { } co) {
            return visitor.Visit(co);
        }

        if (context.logicalOp() is { } lo) {
            return visitor.Visit(lo);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, UnaryOpContext context) {
        if (context.children.First() is ITerminalNode tn) {
            return new OperatorNode(Helpers.MapOperator(tn.Symbol.Type));
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ArithmeticOpContext context) {
        if (context.children.First() is ITerminalNode tn) {
            return new OperatorNode(Helpers.MapOperator(tn.Symbol.Type));
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ConditionalOpContext context) {
        if (context.children.First() is ITerminalNode tn) {
            return new OperatorNode(Helpers.MapOperator(tn.Symbol.Type));
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, LogicalOpContext context) {
        if (context.children.First() is ITerminalNode tn) {
            return new OperatorNode(Helpers.MapOperator(tn.Symbol.Type));
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ProceduralControlFlowContext context) {
        if (context.@if() is { } ic) {
            return visitor.Visit(ic);
        }

        if (context.@while() is { } w) {
            return visitor.Visit(w);
        }

        if (context.repeat() is { } r) {
            return visitor.Visit(r);
        }

        if (context.@for() is { } f) {
            return visitor.Visit(f);
        }

        if (context.@foreach() is { } fi) {
            return visitor.Visit(fi);
        }

        if (context.@switch() is { } sw) {
            return visitor.Visit(sw);
        }

        if (context.@try() is { } t) {
            return visitor.Visit(t);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, IfContext context) {
        var expressions = context.expression().Select(visitor.Visit);
        var statementBlocks = context.statementBlock().Select(visitor.Visit);

        return new IfStatementNode(expressions.ToImmutableArray(), statementBlocks.ToImmutableArray());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, SwitchContext context) {
        var expression = visitor.Visit(context.expression());
        var cases = context.@case().Select(visitor.Visit);
        var defaultCase = visitor.Visit(context.caseDefault());

        return new SwitchStatementNode(expression, cases.ToImmutableArray(), defaultCase);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, WhileContext context) {
        var expression = visitor.Visit(context.expression());
        var statementBlock = visitor.Visit(context.statementBlock());

        return new WhileStatementNode(expression, statementBlock);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ForContext context) {
        var id = visitor.Visit(context.IDENTIFIER());
        var expressions = context.expression().Select(visitor.Visit);
        var statementBlock = visitor.Visit(context.statementBlock());
        var step = context.LITERAL_INTEGER() is { } i ? visitor.Visit(i) : null;
        var neg = context.MINUS() is not null;

        return new ForStatementNode(id, expressions.ToImmutableArray(), step, neg, statementBlock);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ForeachContext context) {
        var id = visitor.Visit(context.IDENTIFIER());
        var expression = visitor.Visit(context.expression());
        var statementBlock = visitor.Visit(context.statementBlock());

        return new ForInStatementNode(id, expression, statementBlock);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, RepeatContext context) {
        var expression = visitor.Visit(context.expression());
        var statementBlock = visitor.Visit(context.statementBlock());

        return new RepeatStatementNode(expression, statementBlock);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, LiteralListContext context) {
        var items = context.literal().Select(visitor.Visit);

        return new LiteralListNode(items.ToImmutableArray());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, LiteralTupleContext context) {
        var items = context.literal().Select(visitor.Visit);

        return new LiteralTupleNode(items.ToImmutableArray());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, LiteralDictionaryContext context) {
        var items = context.literalKvp().Select(visitor.Visit);

        return new LiteralDictionaryNode(items.ToImmutableArray());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, IndexContext context) {
        if (context.range() is { } rangeContext) {
            return visitor.Visit(rangeContext);
        }

        var expressions = context.expression();

        return expressions.Length switch {
            1 => visitor.Visit(expressions.Single()),
            2 => new TwoDIndexExpressionNode(visitor.Visit(expressions.First()), visitor.Visit(expressions.Last())),
            _ => throw new NotImplementedException(context.children.First().GetText())
        };
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, RangeContext context) {
        var prefix = context.children.First() is ITerminalNode;
        var expr1 = visitor.Visit(context.children[prefix ? 1 : 0]);
        var expr2 = context.ChildCount == 3 ? visitor.Visit(context.children[2]) : null;

        return new RangeExpressionNode(prefix, expr1, expr2);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, NewInstanceContext context) {
        var type = visitor.Visit(context.type());
        var args = context.argumentList() is { } al ? al.expression().Select(visitor.Visit) : Array.Empty<IAstNode>();
        var with = (context.withClause() is { } wc ? wc.inlineAsignment().Select(visitor.Visit) : Array.Empty<IAstNode>()).ToImmutableArray();

        var nin = new NewInstanceNode(type, args.ToImmutableArray());

        return with.Any() ? new WithNode(nin, with) : nin;
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, TypeContext context) {
        if (context.dataStructureType() is { } dst) {
            return visitor.Visit(dst);
        }

        if (context.VALUE_TYPE() is { } vt) {
            return new ValueTypeNode(Helpers.MapValueType(vt.GetText()));
        }

        if (context.TYPENAME() is { } tn) {
            var typeName = visitor.Visit(tn);
            return new TypeNode(typeName);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, DataStructureTypeContext context) {
        var genericTypes = context.genericSpecifier().type().Select(visitor.Visit);

        if (context.LIST() is not null) {
            return new DataStructureTypeNode(DataStructure.List, genericTypes.ToImmutableArray());
        }

        if (context.ARRAY() is not null) {
            return new DataStructureTypeNode(DataStructure.Array, genericTypes.ToImmutableArray());
        }

        if (context.DICTIONARY() is not null) {
            return new DataStructureTypeNode(DataStructure.Dictionary, genericTypes.ToImmutableArray());
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, DataStructureDefinitionContext context) {
        if (context.arrayDefinition() is { } ad) {
            return visitor.Visit(ad);
        }

        if (context.listDefinition() is { } ld) {
            return visitor.Visit(ld);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ArrayDefinitionContext context) {
        if (context.genericSpecifier() is { } gs) {
            var genericSpecifier = visitor.Visit(gs);
            var type = new DataStructureTypeNode(DataStructure.Array, ImmutableArray.Create(genericSpecifier));
            var args = ImmutableArray<IAstNode>.Empty;

            if (context.LITERAL_INTEGER() is { } i) {
                args = args.Add(new ValueNode(i.Symbol.Text, new ValueTypeNode(ValueType.Int)));
            }

            return new NewInstanceNode(type, args);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ListDefinitionContext context) {
        var exprs = context.expression().Select(visitor.Visit);

        return new LiteralListNode(exprs.ToImmutableArray());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ProcedureDefContext context) {
        var signature = visitor.Visit(context.procedureSignature());
        var statementBlock = visitor.Visit(context.statementBlock());

        return new ProcedureDefNode(signature, statementBlock);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, FunctionDefContext context) {
        if (context.functionWithBody() is { } fwb) {
            return visitor.Visit(fwb);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, FunctionWithBodyContext context) {
        var signature = visitor.Visit(context.functionSignature());
        var statementBlock = visitor.Visit(context.statementBlock());
        var ret = new ReturnExpressionNode(visitor.Visit(context.expression()));

        return new FunctionDefNode(signature, statementBlock, ret);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, SystemAccessorContext context) {
        var signature = visitor.Visit(context.accessorSignature());
        var statementBlock = visitor.Visit(context.statementBlock());
        var ret = new ReturnExpressionNode(visitor.Visit(context.expression()));

        return new SystemAccessorDefNode(signature, statementBlock, ret);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ProcedureSignatureContext context) {
        var id = visitor.Visit(context.IDENTIFIER());

        if (context.procedureParameterList() is { } pl) {
            var parameters = pl.parameter().Select(visitor.Visit).OfType<ParameterNode>().Select((p, i) => p with { IsRef = pl.OUT(i) is not null }).Cast<IAstNode>();

            return new MethodSignatureNode(id, parameters.ToImmutableArray());
        }

        return new MethodSignatureNode(id, ImmutableArray<IAstNode>.Empty);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, FunctionSignatureContext context) {
        var id = visitor.Visit(context.IDENTIFIER());
        var parameters = context.parameterList() is { } pl ? pl.parameter().Select(visitor.Visit) : Array.Empty<IAstNode>();
        var ret = visitor.Visit(context.type());

        return new MethodSignatureNode(id, parameters.ToImmutableArray(), ret);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, AccessorSignatureContext context) {
        var id = visitor.Visit(context.IDENTIFIER());
        var parameters = context.parameterList() is { } pl ? pl.parameter().Select(visitor.Visit) : Array.Empty<IAstNode>();
        var ret = visitor.Visit(context.type());

        return new MethodSignatureNode(id, parameters.ToImmutableArray(), ret);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ParameterContext context) {
        var id = visitor.Visit(context.IDENTIFIER());
        var type = visitor.Visit(context.type());

        return new ParameterNode(id, type);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, CaseContext context) {
        var val = visitor.Visit(context.literalValue());
        var statementBlock = visitor.Visit(context.statementBlock());

        return new CaseNode(statementBlock, val);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, CaseDefaultContext context) {
        var statementBlock = visitor.Visit(context.statementBlock());

        return new CaseNode(statementBlock);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, TryContext context) {
        var statementBlocks = context.statementBlock().Select(visitor.Visit).ToArray();
        var id = visitor.Visit(context.IDENTIFIER());

        return new TryCatchNode(statementBlocks.First(), id, statementBlocks.Last());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, EnumDefContext context) {
        var ids = context.IDENTIFIER().Select(visitor.Visit);
        var type = visitor.Visit(context.enumType());

        return new EnumDefNode(type, ids.ToImmutableArray());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, EnumValueContext context) {
        var type = visitor.Visit(context.enumType());
        var id = context.IDENTIFIER();

        return new EnumValueNode(id.GetText(), type);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, EnumTypeContext context) => new TypeNode(visitor.Visit(context.TYPENAME()));

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, GenericSpecifierContext context) => visitor.Visit(context.type().Single());

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ClassDefContext context) {
        if (context.mutableClass() is { } mc) {
            return visitor.Visit(mc);
        }

        if (context.immutableClass() is { } ic) {
            return visitor.Visit(ic);
        }

        if (context.abstractClass() is { } ac) {
            return visitor.Visit(ac);
        }

        if (context.abstractImmutableClass() is { } aic) {
            return visitor.Visit(aic);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, MutableClassContext context) {
        var typeName = visitor.Visit(context.TYPENAME());
        var inherits = context.inherits() is { } ih ? ih.type().Select(visitor.Visit) : Array.Empty<IAstNode>();
        var constructor = visitor.Visit(context.constructor());
        var properties = context.property().Select(visitor.Visit);
        var functions = context.functionDef().Select(fd => visitor.Visit<FunctionDefNode>(fd) with { Standalone = false }).Cast<IAstNode>();
        var procedures = context.procedureDef().Select(fd => visitor.Visit<ProcedureDefNode>(fd) with { Standalone = false }).Cast<IAstNode>();

        return new ClassDefNode(typeName, inherits.ToImmutableArray(), constructor, properties.ToImmutableArray(), functions.Concat(procedures).ToImmutableArray(), false);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ImmutableClassContext context) {
        var typeName = visitor.Visit(context.TYPENAME());
        var inherits = context.inherits() is { } ih ? ih.type().Select(visitor.Visit) : Array.Empty<IAstNode>();
        var constructor = visitor.Visit(context.constructor());
        var properties = context.property().Select(visitor.Visit);
        var functions = context.functionDef().Select(fd => visitor.Visit<FunctionDefNode>(fd) with { Standalone = false }).Cast<IAstNode>();

        return new ClassDefNode(typeName, inherits.ToImmutableArray(), constructor, properties.ToImmutableArray(), functions.ToImmutableArray(), true);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, AbstractClassContext context) {
        var typeName = visitor.Visit(context.TYPENAME());
        var inherits = context.inherits() is { } ih ? ih.type().Select(visitor.Visit) : Array.Empty<IAstNode>();
        var properties = context.property().Select(visitor.Visit);
        var functions = context.functionSignature().Select(visitor.Visit);
        var procedures = context.procedureSignature().Select(visitor.Visit);

        return new AbstractClassDefNode(typeName, inherits.ToImmutableArray(), properties.ToImmutableArray(), functions.Concat(procedures).ToImmutableArray());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, AbstractImmutableClassContext context) {
        var typeName = visitor.Visit(context.TYPENAME());
        var inherits = context.inherits() is { } ih ? ih.type().Select(visitor.Visit) : Array.Empty<IAstNode>();
        var properties = context.property().Select(visitor.Visit);
        var functions = context.functionSignature().Select(visitor.Visit);

        return new AbstractClassDefNode(typeName, inherits.ToImmutableArray(), properties.ToImmutableArray(), functions.ToImmutableArray());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ConstructorContext context) {
        var parameters = context.parameterList() is { } pl ? pl.parameter().Select(visitor.Visit) : Array.Empty<IAstNode>();
        var body = visitor.Visit(context.statementBlock());

        return new ConstructorNode(parameters.ToImmutableArray(), body);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, PropertyContext context) {
        var id = visitor.Visit(context.IDENTIFIER());
        var type = visitor.Visit(context.type());
        var isPrivate = context.PRIVATE() is not null;

        return new PropertyDefNode(id, type, isPrivate);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ScopeQualifierContext context) {
        if (context.GLOBAL() is not null) {
            return new GlobalNode();
        }

        if (context.SELF() is not null) {
            return new SelfNode();
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, DeconstructedTupleContext context) {
        var ids = context.IDENTIFIER().Select(visitor.Visit).ToImmutableArray();
        var isNew = Enumerable.Range(0, ids.Length).Select(i => false);

        return new DeconstructionNode(ids, isNew.ToImmutableArray());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ThrowExceptionContext context) {
        var i = context.IDENTIFIER() is { } id ? visitor.Visit(id) : null;
        var s = context.LITERAL_STRING() is { } ls ? new ValueNode(ls.Symbol.Text, new ValueTypeNode(ValueType.String)) : null;

        return new ThrowNode(i ?? s ?? throw new NullReferenceException());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, PrintStatementContext context) {
        var expr = context.expression() is { } e ? visitor.Visit(e) : null;

        return new PrintNode(expr);
    }
}