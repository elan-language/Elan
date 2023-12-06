using AbstractSyntaxTree.Nodes;
using AbstractSyntaxTree;
using SymbolTable.SymbolTypes;

namespace SymbolTable; 

public class SymbolHelpers {
   
     public static ISymbolType MapNodeToSymbolType(IAstNode node) {
        return node switch {
            IdentifierNode idn => new PendingResolveSymbol(idn.Id),
            TypeNode tn => MapNodeToSymbolType(tn.TypeName),
            NewInstanceNode nin => MapNodeToSymbolType(nin.Type),
            ValueNode vn => MapNodeToSymbolType(vn.TypeNode),
            ValueTypeNode vtn => MapElanValueTypeToSymbolType(vtn.Type.ToString()),
            LiteralTupleNode ltn => new TupleSymbolType(ltn.ItemNodes.Select(MapNodeToSymbolType).ToArray()),
            LiteralListNode lln => new ListSymbolType(MapNodeToSymbolType(lln.ItemNodes.First())),
            LiteralDictionaryNode => new DictionarySymbolType(),
            DataStructureTypeNode { Type: DataStructure.Iter } => new IterSymbolType(),
            DataStructureTypeNode { Type: DataStructure.Array } => new ArraySymbolType(),
            DataStructureTypeNode { Type: DataStructure.List } dsn => new ListSymbolType(MapNodeToSymbolType(dsn.GenericTypes.Single())),
            DataStructureTypeNode { Type: DataStructure.Dictionary } => new DictionarySymbolType(),
            MethodCallNode mcn => new PendingResolveSymbol(mcn.Name),
            BinaryNode { Operator: OperatorNode op } when OperatorEvaluatesToBoolean(op.Value) => BoolSymbolType.Instance,
            BinaryNode bn => MapNodeToSymbolType(bn.Operand1),
            IndexedExpressionNode ien => MapNodeToSymbolType(ien.Expression),
            BracketNode bn => MapNodeToSymbolType(bn.BracketedNode),
            UnaryNode un => MapNodeToSymbolType(un.Operand),
            EnumValueNode en => MapNodeToSymbolType(en.TypeNode),
            DefaultNode dn => MapNodeToSymbolType(dn.Type),
            WithNode wn => MapNodeToSymbolType(wn.Expression),
            PropertyCallNode pn => MapNodeToSymbolType(pn.Expression),
            ReturnExpressionNode ren => MapNodeToSymbolType(ren.Expression),
            QualifiedNode qn => MapNodeToSymbolType(qn.Qualified),
            LambdaTypeNode fn => new LambdaSymbolType(fn.Types.Select(MapNodeToSymbolType).ToArray(), MapNodeToSymbolType(fn.ReturnType)),
            MethodSignatureNode { ReturnType: not null } msn => MapNodeToSymbolType(msn.ReturnType),
            InputNode => StringSymbolType.Instance,
            TupleTypeNode ttn => new TupleSymbolType(ttn.Types.Select(MapNodeToSymbolType).ToArray()),
            TupleDefNode ttn => new TupleSymbolType(ttn.Expressions.Select(MapNodeToSymbolType).ToArray()),
            _ => throw new NotImplementedException(node.GetType().ToString())
        };
    }

     private static bool OperatorEvaluatesToBoolean(Operator op) =>
         op switch {
             Operator.And => true,
             Operator.Plus => false,
             Operator.Minus => false,
             Operator.Multiply => false,
             Operator.Divide => false,
             Operator.Power => false,
             Operator.Modulus => false,
             Operator.IntDivide => false,
             Operator.Equal => true,
             Operator.Or => true,
             Operator.Xor => true,
             Operator.LessThan => true,
             Operator.GreaterThanEqual => true,
             Operator.GreaterThan => true,
             Operator.LessThanEqual => true,
             Operator.NotEqual => true,
             _ => throw new ArgumentOutOfRangeException(nameof(op), op, null)
         };

     private static ISymbolType MapElanValueTypeToSymbolType(string type) =>
         type switch {
             IntSymbolType.Name => IntSymbolType.Instance,
             StringSymbolType.Name => StringSymbolType.Instance,
             FloatSymbolType.Name => FloatSymbolType.Instance,
             BoolSymbolType.Name => BoolSymbolType.Instance,
             CharSymbolType.Name => CharSymbolType.Instance,
             _ => throw new NotImplementedException()
         };

}