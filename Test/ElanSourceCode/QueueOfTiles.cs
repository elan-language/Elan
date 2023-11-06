using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals
{
    public record class QueueOfTiles
    {
        public static QueueOfTiles DefaultInstance { get; } = new QueueOfTiles._DefaultQueueOfTiles();
        private QueueOfTiles() { }
        public QueueOfTiles(int maxSize)
        {
            this.maxSize = maxSize;
            contents = new StandardLibrary.ElanArray<string>(maxSize);
            rear = -1;
        }
        protected virtual StandardLibrary.ElanArray<string> contents { get; set; } = StandardLibrary.ElanArray<string>.DefaultInstance;
        protected virtual int rear { get; set; } = default;
        protected virtual int maxSize { get; set; } = default;
        public virtual Boolean isNotEmpty()
        {

            return rear != -1;
        }
        public virtual string show()
        {
            var result = @$"";
            foreach (var letter in contents)
            {
                result = result + letter;
            }
            return result + newline;
        }
        public virtual string asString()
        {

            return StandardLibrary.Functions.typeAndProperties(this);
        }
        public virtual void initialise()
        {
            rear = -1;
            for (var count = 0; count <= maxSize - 2; count = count + 1)
            {
                add();
            }
        }
        public virtual void withdrawNextLetter(ref string letter)
        {
            if (isNotEmpty())
            {
                letter = contents[0];
                for (var count = 1; count <= rear; count = count + 1)
                {
                    contents[count - 1] = contents[count];
                    contents[rear] = @$"";
                }
                rear = rear - 1;
            }
        }
        public virtual void add()
        {
            if (rear < maxSize - 1)
            {
                rear = rear + 1;
                var n = random(0, 30);
                contents[rear] = asString(letters[n]);
            }
        }
        private record class _DefaultQueueOfTiles : QueueOfTiles
        {
            public _DefaultQueueOfTiles() { }
            protected override StandardLibrary.ElanArray<string> contents => StandardLibrary.ElanArray<string>.DefaultInstance;
            protected override int rear => default;
            protected override int maxSize => default;
            public override void initialise() { }
            public override void withdrawNextLetter(ref string letter) { }
            public override void add() { }
            public override string asString() { return "default QueueOfTiles"; }
        }
    }
}

public static class Program
{
    private static void Main(string[] args)
    {
        var q = new QueueOfTiles(10);
        printLine(q.show());
    }
}