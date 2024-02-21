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
            FunctionCallContext c => visitor.Build(c),
            ProcedureCallContext c => visitor.Build(c),
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
            EachContext c => visitor.Build(c),
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
            ProcedureSignatureContext c => visitor.Build(c),
            FunctionSignatureContext c => visitor.Build(c),
            ParameterContext c => visitor.Build(c),
            CaseContext c => visitor.Build(c),
            CaseDefaultContext c => visitor.Build(c),
            TryContext c => visitor.Build(c),
            EnumDefContext c => visitor.Build(c),
            EnumTypeContext c => visitor.Build(c),
            TupleTypeContext c => visitor.Build(c),
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
            InputContext c => visitor.Build(c),
            ProcedureParameterContext c => visitor.Build(c),
            SystemCallContext c => visitor.Build(c),
            FuncTypeContext c => visitor.Build(c),
            ArgumentContext c => visitor.Build(c),
            LambdaContext c => visitor.Build(c),
            ListDecompContext c => visitor.Build(c),
            TupleDefinitionContext c => visitor.Build(c),
            ElseExpressionContext c => visitor.Build(c),
            IfExpressionContext c => visitor.Build(c),
            TestContext c => visitor.Build(c),
            TestStatementsContext c => visitor.Build(c),
            AssertContext c => visitor.Build(c),

            _ => throw new NotImplementedException(context.GetType().FullName ?? null)
        };

    public static IAstNode BuildTerminal(this ElanBaseVisitor<IAstNode> visitor, ITerminalNode node) => new IdentifierNode(node.Symbol.Text, node.Symbol.Line, node.Symbol.Column);

    private static FileNode Build(this ElanBaseVisitor<IAstNode> visitor, FileContext context) {
        var constants = context.constantDef().Select(visitor.Visit);
        var procedures = context.procedureDef().Select(visitor.Visit);
        var functions = context.functionDef().Select(visitor.Visit);
        var enums = context.enumDef().Select(visitor.Visit);
        var classes = context.classDef().Select(visitor.Visit);
        var globals = constants.Concat(procedures).Concat(functions).Concat(enums).Concat(classes).ToImmutableArray();
        var tests = context.test().Select(visitor.Visit).ToImmutableArray();
        var mainNode = context.main().Select(visitor.Visit<MainNode>).SingleOrDefault();

        return new FileNode(globals, mainNode, tests, 0, 0);
    }

    private static StatementBlockNode Build(this ElanBaseVisitor<IAstNode> visitor, StatementBlockContext context) {
        var statements = context.children is { } c ? c.Select(visitor.Visit) : Array.Empty<IAstNode>();
        return new StatementBlockNode(statements.ToImmutableArray(), 0, 0);
    }

    private static MainNode Build(this ElanBaseVisitor<IAstNode> visitor, MainContext context) {
        var block = visitor.Visit<StatementBlockNode>(context.statementBlock());

        return new MainNode(block, 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, CallStatementContext context) {
        if (context.DOT() is not null) {
            var ms = visitor.Visit<ProcedureCallNode>(context.procedureCall());
            var exp = visitor.Visit(context.assignableValue());

            return new CallStatementNode(ms with { CalledOn = exp }, 0, 0);
        }

        return new CallStatementNode(visitor.Visit<ProcedureCallNode>(context.procedureCall()), 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ExpressionContext context) {
        if (context.DOT() is not null) {
            if (context.functionCall() is { } dmc) {
                var ms = visitor.Visit<FunctionCallNode>(dmc);
                var exp = visitor.Visit(context.expression().First());

                return ms with { CalledOn = exp };
            }

            if (context.IDENTIFIER() is { } id) {
                var exp = visitor.Visit(context.expression().First());
                var prop = visitor.Visit(id);
                return new PropertyCallNode(exp, prop, 0, 0);
            }
        }

        if (context.functionCall() is { } mc) {
            return visitor.Visit(mc);
        }

        if (context.binaryOp() is { } bop) {
            return new BinaryNode(visitor.Visit(bop), visitor.Visit(context.expression().First()), visitor.Visit(context.expression().Last()), 0, 0);
        }

        if (context.POWER() is { } p) {
            var op = new OperatorNode(Helpers.MapOperator(p.Symbol.Type), 0, 0);
            return new BinaryNode(op, visitor.Visit(context.expression().First()), visitor.Visit(context.expression().Last()), 0, 0);
        }

        if (context.value() is { } v) {
            return visitor.Visit(v);
        }

        if (context.bracketedExpression() is { } b) {
            return new BracketNode(visitor.Visit(b.expression()), 0, 0);
        }

        if (context.unaryOp() is { } uop) {
            var symbol = (uop.MINUS() ?? uop.NOT()).Symbol;
            return new UnaryNode(visitor.Visit(uop), visitor.Visit(context.expression().Single()), symbol.Line, symbol.Column);
        }

        if (context.index() is { } idx) {
            var expr = visitor.Visit(context.expression().Single());
            var range = visitor.Visit(idx);
            var symbol = idx.OPEN_SQ_BRACKET().Symbol;
            return new IndexedExpressionNode(expr, range, symbol.Line, symbol.Column);
        }

        if (context.newInstance() is { } ni) {
            return visitor.Visit(ni);
        }

        if (context.withClause() is { } wc) {
            var expr = visitor.Visit(context.expression().Single());
            var with = wc.inlineAsignment().Select(visitor.Visit);
            var symbol = wc.WITH().Symbol;
            return new WithNode(expr, with.ToImmutableArray(), symbol.Line, symbol.Column);
        }

        if (context.elseExpression() is { } tre) {
            var expr = visitor.Visit(context.expression().Single());
            var ifExpr = visitor.Visit(context.ifExpression());
            var elseExpr = visitor.Visit(tre);
            return new IfExpressionNode(new[] {expr, ifExpr, elseExpr}.ToImmutableArray(), expr.Line, expr.Column);
        }

        if (context.systemCall() is { } sc) {
            return visitor.Visit(sc);
        }

        if (context.input() is { } inp) {
            return visitor.Visit(inp);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ProcedureCallContext context) {
        var id = visitor.Visit(context.IDENTIFIER());
        var pps = (context.argumentList()?.argument().Select(visitor.Visit) ?? Array.Empty<IAstNode>()).ToImmutableArray();
        var sq = context.scopeQualifier() is { } s ? visitor.Visit(s) : null;

        return new ProcedureCallNode(id, sq, pps, null, 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, FunctionCallContext context) {
        var id = visitor.Visit(context.IDENTIFIER());
        var pps = (context.argumentList()?.argument().Select(visitor.Visit) ?? Array.Empty<IAstNode>()).ToImmutableArray();
        var sq = context.scopeQualifier() is { } s ? visitor.Visit(s) : null;

        return new FunctionCallNode(id, sq, pps, null, 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, SystemCallContext context) {
        var id = visitor.Visit(context.IDENTIFIER());
        var pps = (context.argumentList()?.argument().Select(visitor.Visit) ?? Array.Empty<IAstNode>()).ToImmutableArray();

        return new SystemAccessorCallNode(id, new SystemAccessorPrefixNode(0, 0), pps, 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ValueContext context) {
        if (context.scopeQualifier() is { } nq) {
            var nqn = visitor.Visit(nq);
            var idn = visitor.Visit(context.IDENTIFIER());
            return new QualifiedNode(nqn, idn, 0, 0);
        }

        if (context.literal() is { } lv) {
            return visitor.Visit(lv);
        }

        if (context.IDENTIFIER() is { } id) {
            return visitor.Visit(id);
        }

        if (context.dataStructureDefinition() is { } ds) {
            return visitor.Visit(ds);
        }

        if (context.THIS() is not null) {
            return new ThisInstanceNode(0,0);
        }

        if (context.DEFAULT() is not null) {
            return new DefaultNode(visitor.Visit(context.type()), 0, 0);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, VarDefContext context) {
        var id = visitor.Visit(context.assignableValue());
        var rhs = visitor.Visit(context.expression());

        return new VarDefNode(id, rhs, 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ConstantDefContext context) {
        var id = visitor.Visit(context.IDENTIFIER());
        var expr = context.literal() is { } l ? visitor.Visit(l) : null;
        var newInstance = context.newInstance() is { } n ? visitor.Visit(n) : null;

        return new ConstantDefNode(id, expr ?? newInstance!, 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, AssignmentContext context) {
        var id = visitor.Visit(context.assignableValue());
        var rhs = visitor.Visit(context.expression());

        return new AssignmentNode(id, rhs, false, 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, InlineAsignmentContext context) {
        var id = visitor.Visit(context.assignableValue());
        var expr = visitor.Visit(context.expression());

        return new AssignmentNode(id, expr, true, 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, AssignableValueContext context) {
        if (context.index() is { } idx) {
            var index = visitor.Visit(idx);
            var expr = visitor.Visit(context.IDENTIFIER());

            return new IndexedExpressionNode(expr, index, 0, 0);
        }

        if (context.scopeQualifier() is { } nq) {
            var qual = visitor.Visit(nq);
            var expr = visitor.Visit(context.IDENTIFIER());

            return new QualifiedNode(qual, expr, 0, 0);
        }

        if (context.IDENTIFIER() is { } id) {
            return visitor.Visit(id);
        }

        if (context.deconstructedTuple() is { } dt) {
            return visitor.Visit(dt);
        }

        if (context.listDecomp() is { } dl) {
            return visitor.Visit(dl);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, LiteralValueContext context) {
        if (context.LITERAL_INTEGER() is { } i) {
            return new ValueNode(i.Symbol.Text, new ValueTypeNode(ValueType.Int, i.Symbol.Line, i.Symbol.Column), i.Symbol.Line, i.Symbol.Column);
        }

        if (context.LITERAL_FLOAT() is { } f) {
            return new ValueNode(f.Symbol.Text, new ValueTypeNode(ValueType.Float, f.Symbol.Line, f.Symbol.Column), f.Symbol.Line, f.Symbol.Column);
        }

        if (context.LITERAL_CHAR() is { } c) {
            return new ValueNode(c.Symbol.Text, new ValueTypeNode(ValueType.Char, c.Symbol.Line, c.Symbol.Column), c.Symbol.Line, c.Symbol.Column);
        }

        if (context.BOOL_VALUE() is { } b) {
            return new ValueNode(b.Symbol.Text, new ValueTypeNode(ValueType.Bool,b.Symbol.Line, b.Symbol.Column), b.Symbol.Line, b.Symbol.Column);
        }

        if (context.enumValue() is { } ev) {
            return visitor.Visit(ev);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, LiteralKvpContext context) {
        var key = visitor.Visit(context.literal().First());
        var value = visitor.Visit(context.literal().Last());

        return new PairNode(key, value, 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, LiteralDataStructureContext context) {
        if (context.LITERAL_STRING() is { } ls) {
            return new ValueNode(ls.Symbol.Text, new ValueTypeNode(ValueType.String,ls.Symbol.Line, ls.Symbol.Column), ls.Symbol.Line, ls.Symbol.Column);
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
            return new OperatorNode(Helpers.MapOperator(tn.Symbol.Type), 0, 0);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ArithmeticOpContext context) {
        if (context.children.First() is ITerminalNode tn) {
            return new OperatorNode(Helpers.MapOperator(tn.Symbol.Type), 0, 0);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ConditionalOpContext context) {
        if (context.children.First() is ITerminalNode tn) {
            return new OperatorNode(Helpers.MapOperator(tn.Symbol.Type), 0, 0);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, LogicalOpContext context) {
        if (context.children.First() is ITerminalNode tn) {
            return new OperatorNode(Helpers.MapOperator(tn.Symbol.Type), 0, 0);
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

        if (context.each() is { } fi) {
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

        return new IfStatementNode(expressions.ToImmutableArray(), statementBlocks.ToImmutableArray(), 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, SwitchContext context) {
        var expression = visitor.Visit(context.expression());
        var cases = context.@case().Select(visitor.Visit);
        var defaultCase = visitor.Visit(context.caseDefault());
        var symbol = context.SWITCH(0).Symbol;

        return new SwitchStatementNode(expression, cases.ToImmutableArray(), defaultCase, symbol.Line, symbol.Column);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, WhileContext context) {
        var expression = visitor.Visit(context.expression());
        var statementBlock = visitor.Visit(context.statementBlock());
        var symbol = context.WHILE(0).Symbol;

        return new WhileStatementNode(expression, statementBlock, symbol.Line, symbol.Column);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ForContext context) {
        var id = visitor.Visit(context.IDENTIFIER());
        var expressions = context.expression().Select(visitor.Visit);
        var statementBlock = visitor.Visit(context.statementBlock());
        var step = context.LITERAL_INTEGER() is { } i ? visitor.Visit(i) : null;
        var neg = context.MINUS() is not null;
        var symbol = context.FOR(0).Symbol;

        return new ForStatementNode(id, expressions.ToImmutableArray(), step, neg, statementBlock, symbol.Line, symbol.Column);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, EachContext context) {
        var idToken = context.IDENTIFIER();
        var id = visitor.Visit(idToken);
        var expression = visitor.Visit(context.expression());
        var statementBlock = visitor.Visit(context.statementBlock());
        var symbol = context.EACH(0).Symbol;

        return new EachStatementNode(new EachParameterNode(id, expression, idToken.Symbol.Line, idToken.Symbol.Column), statementBlock, symbol.Line, symbol.Column);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, RepeatContext context) {
        var expression = visitor.Visit(context.expression());
        var statementBlock = visitor.Visit(context.statementBlock());
        var symbol = context.REPEAT(0).Symbol;

        return new RepeatStatementNode(expression, statementBlock, symbol.Line, symbol.Column);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, LiteralListContext context) {
        var items = context.literal().Select(visitor.Visit);
        var symbol = context.OPEN_BRACE().Symbol;

        return new LiteralListNode(items.ToImmutableArray(), symbol.Line, symbol.Column);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, LiteralTupleContext context) {
        var items = context.literal().Select(visitor.Visit);
        var symbol = context.OPEN_BRACKET().Symbol;
        return new LiteralTupleNode(items.ToImmutableArray(), symbol.Line, symbol.Column);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, LiteralDictionaryContext context) {
        var items = context.literalKvp().Select(visitor.Visit);
        var symbol = context.OPEN_BRACE().Symbol;
        return new LiteralDictionaryNode(items.ToImmutableArray(), symbol.Line, symbol.Column);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, IndexContext context) {
        if (context.range() is { } rangeContext) {
            return visitor.Visit(rangeContext);
        }

        var expressions = context.expression();
        var symbol = context.OPEN_SQ_BRACKET().Symbol;

        return expressions.Length switch {
            1 => visitor.Visit(expressions.Single()),
            2 => new TwoDIndexExpressionNode(visitor.Visit(expressions.First()), visitor.Visit(expressions.Last()), symbol.Line, symbol.Column),
            _ => throw new NotImplementedException(context.children.First().GetText())
        };
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, RangeContext context) {
        var prefix = context.children.First() is ITerminalNode;
        var expr1 = visitor.Visit(context.children[prefix ? 1 : 0]);
        var expr2 = context.ChildCount == 3 ? visitor.Visit(context.children[2]) : null;

        return new RangeExpressionNode(prefix, expr1, expr2, 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, NewInstanceContext context) {
        var type = visitor.Visit(context.type());
        var args = context.argumentList() is { } al ? al.argument().Select(visitor.Visit) : Array.Empty<IAstNode>();
        var symbol = context.NEW().Symbol;
        var nin = new NewInstanceNode(type, args.ToImmutableArray(), symbol.Line, symbol.Column);

        if (context.withClause() is { } wc) {
            var withSymbol = wc.WITH().Symbol;
            var with = wc.inlineAsignment().Select(visitor.Visit).ToImmutableArray();
            return new WithNode(nin, with, withSymbol.Line, withSymbol.Column);
        }

        return nin;
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, TypeContext context) {
        if (context.dataStructureType() is { } dst) {
            return visitor.Visit(dst);
        }

        if (context.VALUE_TYPE() is { } vt) {
            return new ValueTypeNode(Helpers.MapValueType(vt.GetText()), vt.Symbol.Line, vt.Symbol.Column);
        }

        if (context.TYPENAME() is { } tn) {
            var typeName = visitor.Visit(tn);
            return new TypeNode(typeName, tn.Symbol.Line, tn.Symbol.Column);
        }

        if (context.funcType() is { } ft) {
            return visitor.Visit(ft);
        }

        if (context.tupleType() is { } tt) {
            return visitor.Visit(tt);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, DataStructureTypeContext context) {
        var genericTypes = context.genericSpecifier().type().Select(visitor.Visit);

        if (context.ITERABLE() is {} iter) {
            return new DataStructureTypeNode(DataStructure.Iter, genericTypes.ToImmutableArray(), iter.Symbol.Line, iter.Symbol.Column);
        }

        if (context.LIST() is {} lst) {
            return new DataStructureTypeNode(DataStructure.List, genericTypes.ToImmutableArray(), lst.Symbol.Line, lst.Symbol.Column);
        }

        if (context.ARRAY() is {} arr) {
            return new DataStructureTypeNode(DataStructure.Array, genericTypes.ToImmutableArray(), arr.Symbol.Line, arr.Symbol.Column);
        }

        if (context.DICTIONARY() is {} dict) {
            return new DataStructureTypeNode(DataStructure.Dictionary, genericTypes.ToImmutableArray(), dict.Symbol.Line, dict.Symbol.Column);
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

        if (context.tupleDefinition() is { } td) {
            return visitor.Visit(td);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ArrayDefinitionContext context) {
        if (context.genericSpecifier() is { } gs) {
            var genericSpecifier = visitor.Visit(gs);
            var type = new DataStructureTypeNode(DataStructure.Array, ImmutableArray.Create(genericSpecifier), 0, 0);
            var args = ImmutableArray<IAstNode>.Empty;

            if (context.LITERAL_INTEGER() is { } i) {
                args = args.Add(new ValueNode(i.Symbol.Text, new ValueTypeNode(ValueType.Int, i.Symbol.Line, i.Symbol.Column), i.Symbol.Line, i.Symbol.Column));
            }

            return new NewInstanceNode(type, args, 0, 0);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ListDefinitionContext context) {
        var exprs = context.expression().Select(visitor.Visit);

        return new LiteralListNode(exprs.ToImmutableArray(), 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ProcedureDefContext context) {
        var signature = visitor.Visit(context.procedureSignature());
        var statementBlock = visitor.Visit(context.statementBlock());
        var symbol = context.PROCEDURE(0).Symbol;
        return new ProcedureDefNode(signature, statementBlock, true, symbol.Line, symbol.Column);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, FunctionDefContext context) {
        var signature = visitor.Visit(context.functionSignature());
        var statementBlock = visitor.Visit(context.statementBlock());
        var symbol = context.RETURN().Symbol;
        var ret = context.expression() is { } expr
            ? (IAstNode) new ReturnExpressionNode(visitor.Visit(expr), symbol.Line, symbol.Column)
            : signature is MethodSignatureNode { ReturnType: { } rt }
                ? new DefaultNode(rt, symbol.Line, symbol.Column)
                : throw new NotImplementedException(context.children.First().GetText());

        symbol = context.FUNCTION(0).Symbol;
        return new FunctionDefNode(signature, statementBlock, ret, true, symbol.Line, symbol.Column);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ProcedureSignatureContext context) {
        var idToken = context.IDENTIFIER();
        var id = visitor.Visit(idToken);
        var symbol = idToken.Symbol;

        if (context.procedureParameterList() is { } pl) {
            var parameters = pl.procedureParameter().Select(visitor.Visit);

            return new MethodSignatureNode(id, parameters.ToImmutableArray(), null, symbol.Line, symbol.Column);
        }

        return new MethodSignatureNode(id, ImmutableArray<IAstNode>.Empty, null, symbol.Line, symbol.Column);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, FunctionSignatureContext context) {
        var idToken = context.IDENTIFIER();
        var id = visitor.Visit(idToken);
        var symbol = idToken.Symbol;
        var parameters = context.parameterList() is { } pl ? pl.parameter().Select(visitor.Visit) : Array.Empty<IAstNode>();
        var ret = visitor.Visit(context.type());

        return new MethodSignatureNode(id, parameters.ToImmutableArray(), ret, symbol.Line, symbol.Column);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ParameterContext context) {
        var id = visitor.Visit(context.IDENTIFIER());
        var type = visitor.Visit(context.type());

        return new ParameterNode(id, type, false, 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ProcedureParameterContext context) {
        var id = visitor.Visit(context.IDENTIFIER());
        var type = visitor.Visit(context.type());
        var byRef = context.OUT() is not null;

        return new ParameterNode(id, type, byRef, 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, CaseContext context) {
        var val = visitor.Visit(context.literalValue());
        var statementBlock = visitor.Visit(context.statementBlock());

        return new CaseNode(statementBlock, val, 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, CaseDefaultContext context) {
        var statementBlock = visitor.Visit(context.statementBlock());
        var symbol = context.DEFAULT().Symbol;
        return new CaseNode(statementBlock, null, symbol.Line, symbol.Column);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, TryContext context) {
        var statementBlocks = context.statementBlock().Select(visitor.Visit).ToArray();
        var id = visitor.Visit(context.IDENTIFIER());
        var symbol = context.TRY(0).Symbol;
        return new TryCatchNode(statementBlocks.First(), id, statementBlocks.Last(), symbol.Line, symbol.Column);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, EnumDefContext context) {
        var ids = context.IDENTIFIER().Select(visitor.Visit);
        var type = visitor.Visit(context.enumType());

        return new EnumDefNode(type, ids.ToImmutableArray(), 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, EnumValueContext context) {
        var type = visitor.Visit(context.enumType());
        var id = context.IDENTIFIER();

        return new EnumValueNode(id.GetText(), type, 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, EnumTypeContext context) => new TypeNode(visitor.Visit(context.TYPENAME()), 0, 0);

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

        return new ClassDefNode(typeName, inherits.ToImmutableArray(), constructor, properties.ToImmutableArray(), functions.Concat(procedures).ToImmutableArray(), false, 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ImmutableClassContext context) {
        var typeName = visitor.Visit(context.TYPENAME());
        var inherits = context.inherits() is { } ih ? ih.type().Select(visitor.Visit) : Array.Empty<IAstNode>();
        var constructor = visitor.Visit(context.constructor());
        var properties = context.property().Select(visitor.Visit);
        var functions = context.functionDef().Select(fd => visitor.Visit<FunctionDefNode>(fd) with { Standalone = false }).Cast<IAstNode>();

        return new ClassDefNode(typeName, inherits.ToImmutableArray(), constructor, properties.ToImmutableArray(), functions.ToImmutableArray(), true, 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, AbstractClassContext context) {
        var typeName = visitor.Visit(context.TYPENAME());
        var inherits = context.inherits() is { } ih ? ih.type().Select(visitor.Visit) : Array.Empty<IAstNode>();
        var properties = context.property().Select(visitor.Visit);
        var functions = context.functionSignature().Select(visitor.Visit).Select(n => new AbstractFunctionDefNode(n, 0, 0) as IAstNode);
        var procedures = context.procedureSignature().Select(visitor.Visit).Select(n => new AbstractProcedureDefNode(n, 0, 0) as IAstNode);

        return new AbstractClassDefNode(typeName, inherits.ToImmutableArray(), properties.ToImmutableArray(), functions.Concat(procedures).ToImmutableArray(), 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, AbstractImmutableClassContext context) {
        var typeName = visitor.Visit(context.TYPENAME());
        var inherits = context.inherits() is { } ih ? ih.type().Select(visitor.Visit) : Array.Empty<IAstNode>();
        var properties = context.property().Select(visitor.Visit);
        var functions = context.functionSignature().Select(visitor.Visit);

        return new AbstractClassDefNode(typeName, inherits.ToImmutableArray(), properties.ToImmutableArray(), functions.ToImmutableArray(), 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ConstructorContext context) {
        var parameters = context.parameterList() is { } pl ? pl.parameter().Select(visitor.Visit) : Array.Empty<IAstNode>();
        var body = visitor.Visit(context.statementBlock());

        return new ConstructorNode(parameters.ToImmutableArray(), body, 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, PropertyContext context) {
        var id = visitor.Visit(context.IDENTIFIER());
        var type = visitor.Visit(context.type());
        var isPrivate = context.PRIVATE() is not null;

        return new PropertyDefNode(id, type, isPrivate, 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ScopeQualifierContext context) {
        if (context.GLOBAL() is not null) {
            return new GlobalPrefixNode(0,0);
        }

        if (context.PROPERTY() is not null) {
            return new PropertyPrefixNode(0,0);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, DeconstructedTupleContext context) {
        var ids = context.IDENTIFIER().Select(visitor.Visit).ToImmutableArray();
        var isNew = Enumerable.Range(0, ids.Length).Select(_ => false);

        return new DeconstructionNode(ids, isNew.ToImmutableArray(), 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ListDecompContext context) {
        var ids = context.IDENTIFIER().Select(visitor.Visit).ToImmutableArray();
        var isNew = Enumerable.Range(0, ids.Length).Select(_ => false);

        return new DeconstructionNode(ids, isNew.ToImmutableArray(), 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ThrowExceptionContext context) {
        var i = context.IDENTIFIER() is { } id ? visitor.Visit(id) : null;
        var s = context.LITERAL_STRING() is { } ls ? new ValueNode(ls.Symbol.Text, new ValueTypeNode(ValueType.String, ls.Symbol.Line, ls.Symbol.Column), ls.Symbol.Line, ls.Symbol.Column) : null;

        return new ThrowNode(i ?? s ?? throw new NullReferenceException(), 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, PrintStatementContext context) {
        var expr = context.expression() is { } e ? visitor.Visit(e) : null;

        return new PrintNode(expr, 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, InputContext context) {
        var prompt = context.LITERAL_STRING() is { } e ? e.Symbol.Text : null;

        return new InputNode(prompt, 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, FuncTypeContext context) {
        var inputTypes = context.typeList().type().Select(visitor.Visit);
        var returnType = visitor.Visit(context.type());

        return new LambdaTypeNode(inputTypes.ToImmutableArray(), returnType, 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, TupleTypeContext context) {
        var inputTypes = context.type().Select(visitor.Visit);

        return new TupleTypeNode(inputTypes.ToImmutableArray(), 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ArgumentContext context) {
        if (context.expression() is { } e) {
            return visitor.Visit(e);
        }

        if (context.lambda() is { } l) {
            return visitor.Visit(l);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, LambdaContext context) {
        var arguments = context.argumentList().argument().Select(a => visitor.Visit(a));
        var expr = visitor.Visit(context.expression());

        return new LambdaDefNode(arguments.ToImmutableArray(), expr, 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, TupleDefinitionContext context) {
        var expression = context.expression().Select(visitor.Visit);

        return new TupleDefNode(expression.ToImmutableArray(), 0, 0);
    }
    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElseExpressionContext context) {
        var expression = visitor.Visit(context.expression());

        return expression;
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, IfExpressionContext context) {
        var expression = visitor.Visit(context.expression());

        return expression;
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, TestContext context) {
        var id = visitor.Visit(context.IDENTIFIER());
        var statements = visitor.Visit(context.testStatements());

        return new TestDefNode(id, statements, 0, 0);
    }

    
    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, TestStatementsContext context) {
        var statements = context.children is { } c ? c.Select(visitor.Visit) : Array.Empty<IAstNode>();
        return new StatementBlockNode(statements.ToImmutableArray(), 0, 0);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, AssertContext context) {
        var expression = visitor.Visit(context.expression());
        var value = visitor.Visit(context.value());
        return new AssertNode(expression, value, 0, 0);
    }
}