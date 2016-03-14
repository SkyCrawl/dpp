using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HuffmanCoding
{
    /// <summary>
    /// The node of a Huffman tree. All properties of the node are read-only.
    /// </summary>
    class HuffNode : IComparable<HuffNode>
    {
        #region Fields

        private static int NODES_CREATED = 0;

        #endregion

        #region Properties

        /// <summary>
        /// The ASCII character, that the node is representing.
        /// </summary>
        public byte Character { get; private set; }

        /// <summary>
        /// The frequency of the character in the document.
        /// </summary>
        public int Frequency { get; private set; }

        /// <summary>
        /// Number of nodes created before this node.
        /// </summary>
        public int Age { get; private set; }

        public HuffNode LeftChild { get; private set; }
        public HuffNode RightChild { get; private set; }

        /// <summary>
        /// Kdyz nema jedineho syna vraci true.
        /// </summary>
        /// <returns></returns>
        public bool IsLeaf
        {
            get { return (LeftChild == null) && (RightChild == null); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <see cref="HuffNode"/>. All values have to be set
        /// here, no value can be changed later.
        /// </summary>
        /// <param name="character">The ASCII character, that the node is
        /// representing.</param>
        /// <param name="frequency">The frequency of the character in the
        /// document.</param>
        /// <param name="leftSon">The left child of the node.</param>
        /// <param name="rightSon">The right child of the node.</param>
        public HuffNode(
            byte character,
            int frequency,
            HuffNode leftSon,
            HuffNode rightSon)
        {
            this.Frequency = frequency;
            this.Character = character;
            this.Age = NODES_CREATED++;
            this.LeftChild = leftSon;
            this.RightChild = rightSon;
        }

        #endregion

        #region Methods

        /// <summary>
        /// True o sobe vrchol rekne jestli bude v Huffmanskem strome nalevo od
        /// druheho vrcholu.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool ShouldBeLeftOf(HuffNode other)
        {
            // check various properties, until we find a difference
            if (this.Frequency != other.Frequency)
            {
                return this.Frequency < other.Frequency;
            }
            else if (this.IsLeaf != other.IsLeaf)
            {
                return this.IsLeaf;
            }
            else if (this.IsLeaf && (this.Character != other.Character))
            {
                return this.Character < other.Character;
            }
            else
            {
                return this.Age < other.Age;
            }
        }

        /* UNUSED
        public static int SumOccurrences(Node node1, Node node2)
        {
            return node1.occurrences + node2.occurrences;
        }

        /// <summary>
        /// Zvetsi vahu vrcholu o zadany int, vraci upraveny vrchol.
        /// </summary>
        /// <param name="byHowMuch"></param>
        /// <returns></returns>
        public Node RaiseOccurrences(int byHowMuch)
        {
            occurrences += byHowMuch;
            return this;
        }
        */

        #endregion

        #region IComparable Members

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being
        /// compared. The return value has the following meanings: Value Meaning
        /// Less than zero This object is less than the
        /// <paramref name="other" /> parameter.Zero This object is equal to
        /// <paramref name="other" />. Greater than zero This object is greater
        /// than <paramref name="other" />.
        /// </returns>
        public int CompareTo(HuffNode other)
        {
            if (this == other)
            {
                return 0;
            }
            else
            {
                return this.ShouldBeLeftOf(other) ? -1 : 1;
            }
        }

        #endregion
    }

    /// <summary>
    /// The huffman tree. There is no easy way to change the tree, it has to be
    /// fully loaded from already existing nodes.
    /// </summary>
    class HuffTree
    {
        #region Fields

        const string EMPTY_TREE_MESSAGE = "Algorithm doesn't allow zipping of"
            + " 'null' nodes. Is the tree construction algorithm broken?";

        /// <summary>
        /// The lowest ASCII value, that is printed as character.
        /// </summary>
        private static byte MIN_ASCII_PRINTABLE = 32;

        /// <summary>
        /// The highest ASCII value, that is printed as character.
        /// </summary>
        private static byte MAX_ASCII_PRINTABLE = 126;

        private HuffNode root;

        #endregion

        #region Classes

        /// <summary>
        /// The internal class, that represents the state of the tree
        /// generation.
        /// </summary>
        private class ConstructionState
        {
            public FreqSortedHuffForest forests;
            public HuffNode lastIterLeftoverTree;

            public bool HasTwoMoreTreesToProcess
            {
                get
                {
                    return forests.HasMoreTreesThan(
                        lastIterLeftoverTree != null ? 0 : 1);
                }
            }

            public ConstructionState(
                FreqSortedHuffForest forests,
                HuffNode lastIterLeftoverTree)
            {
                this.forests = forests;
                this.lastIterLeftoverTree = lastIterLeftoverTree;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <see cref="HuffTree"/>.
        /// All nodes have to be specified here.
        /// </summary>
        /// <param name="forests"></param>
        public HuffTree(FreqSortedHuffForest forests)
        {
            // create the constructor-scoped state variable, shared among
            // related methods
            ConstructionState state = new ConstructionState(forests, null);

            // zip pairs of trees, until there is a single one left (unless the
            // input is empty)
            while (state.HasTwoMoreTreesToProcess)
            {
                int minFreq = ProcessTwoTrees(state);

                // remove the currently processed forest
                state.forests.Remove(minFreq);
            }

            // the last surviving tree is the last added tree with highest
            // frequency number
            // Note: mind empty input...
            this.root = state.forests.LowestTree; // null if empty
        }

        #endregion

        #region Methods

        private int ProcessTwoTrees(ConstructionState state)
        {
            // get the current minimum frequency and corresponding huffman
            // forest
            int minFreq = state.forests.LowestFreq;
            HuffForest minFreqForest = state.forests.ForestWithLowestFreq;

            // branch depending on whether we still have a leftover tree from
            // the last iteration
            IEnumerator<HuffNode> minFreqForestEnumerator =
                minFreqForest.GetEnumerator();

            if (state.lastIterLeftoverTree == null)
            {
                // pair-wise zip of the forest's trees, maybe leaving a leftover
                PairWiseForestZip(minFreqForestEnumerator, state);
            }
            else // we have a leftover tree from last iteration
            {
                // if the forest happens to be empty (else branch), do nothing
                // and remove the forest later on
                if (minFreqForestEnumerator.MoveNext())
                {
                    // then get the first currently processed forest
                    // Both are non-null (guaranteed by condition)
                    HuffNode tree1 = state.lastIterLeftoverTree;
                    HuffNode tree2 = minFreqForestEnumerator.Current;

                    // zip the two trees into one and add it to be processed
                    // later
                    ZipTrees(state, tree1, tree2);

                    // leftover tree has just been zipped => reset the pointer
                    state.lastIterLeftoverTree = null;

                    // and now pair-wise zip of the forest's trees,
                    // maybe leaving a leftover
                    PairWiseForestZip(minFreqForestEnumerator, state);
                }
            }
            return minFreq;
        }

        /// <summary>
        /// Merges next two trees of Huffman nodes into one, that is added back
        /// into the construction state.
        /// </summary>
        /// <param name="enumerator"></param>
        /// <param name="state"></param>
        private void PairWiseForestZip(
            IEnumerator<HuffNode> enumerator,
            ConstructionState state)
        {
            while (enumerator.MoveNext())
            {
                // non-null (guaranteed by condition)
                HuffNode tree1 = enumerator.Current;

                if (enumerator.MoveNext()) // second node is available
                {
                    // non-null (guaranteed by condition)
                    HuffNode tree2 = enumerator.Current;

                    // zip the two trees into one and
                    // add it to be processed later
                    ZipTrees(state, tree1, tree2);
                }
                else // second node is not available
                {
                    // save the first and continue iterating
                    state.lastIterLeftoverTree = tree1;
                }
            }
        }

        private void ZipTrees(
            ConstructionState state,
            HuffNode tree1,
            HuffNode tree2)
        {
            // just-in-case integrity check
            tree1.ThrowIfNull(EMPTY_TREE_MESSAGE);
            tree2.ThrowIfNull(EMPTY_TREE_MESSAGE);

            // zip the trees into one
            bool oneLeftOfTwo = tree1.ShouldBeLeftOf(tree2);
            HuffNode compoundTree = new HuffNode(
                // doesn't matter for inner nodes
                // because they don't represent a character
                tree1.Character,
                tree1.Frequency + tree2.Frequency,
                oneLeftOfTwo ? tree1 : tree2, // one is left
                oneLeftOfTwo ? tree2 : tree1); // and the other is right

            // and add it to be processed later
            state.forests.Add(compoundTree);
        }

        /* UNUSED
        public void Print()
        {
            if(this.root != null)
            {
                VypisStrom(this.root); // FIXME: method doesn't exist
            }
        }
        */

        /// <summary>
        /// Prints the entire tree into standard output.
        /// </summary>
        public void Print()
        {
            this.root.ThrowIfNull("No root. Was the input empty?");
            PrintSubtree(this.root, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subtreeRoot">The subtree,
        /// that should be printed.</param>
        /// <param name="indentation">The prefix that is used before all nodes
        /// of the subtree</param>
        private void PrintSubtree(HuffNode subtreeRoot, string indentation)
        {
            if (subtreeRoot.IsLeaf)
            {
                // print self
                if ((subtreeRoot.Character < MIN_ASCII_PRINTABLE) ||
                    (subtreeRoot.Character > MAX_ASCII_PRINTABLE))
                {
                    Console.Write(
                        " [{0}:{1}]\n",
                        subtreeRoot.Character,
                        subtreeRoot.Frequency);
                }
                else
                {
                    Console.Write(
                        " ['{0}':{1}]\n",
                        (char)subtreeRoot.Character,
                        subtreeRoot.Frequency);
                }
            }
            else
            {
                // first raise indentation
                indentation = indentation + new string(' ', 6);

                // then print self
                Console.Write("{0,4} -+- ", subtreeRoot.Frequency);

                // and finally, print the subtree
                // Note: child null-check is done in the 'ZipTrees' method and
                // doesn't need to be included here
                PrintSubtree(subtreeRoot.RightChild, indentation + "|  ");
                Console.Write("{0}|\n{0}`- ", indentation);
                PrintSubtree(
                    subtreeRoot.LeftChild, indentation + new string(' ', 3));
            }
        }

        #endregion
    }

    /// <summary>
    /// Helper class that reads charater frequencies from an input stream.
    /// </summary>
    class CharToFrequency : Dictionary<byte, int>
    {
        #region Fields

        /// <summary>
        /// The size of the internal buffer used for reading the stream.
        /// </summary>
        private static int BUFF_SIZE = 0x4000; // 16384

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the character frequencies dictionary.
        /// </summary>
        /// <param name="inputStream"></param>
        public CharToFrequency(Stream inputStream)
        {
            // declare buffer
            byte[] buff = new byte[BUFF_SIZE];

            // read and process the file by chunks
            int bytesRead = 0;
            do
            {
                // read
                bytesRead = inputStream.Read(buff, 0, BUFF_SIZE);

                // index each byte read
                for (int i = 0; i < bytesRead; i++)
                {
                    InsertCharacter(buff[i]);
                }
            }
            while (bytesRead == BUFF_SIZE);
        }

        #endregion

        #region Methods

        private void InsertCharacter(byte character)
        {
            if (!this.ContainsKey(character)) // byte not yet indexed
            {
                // index it, number of occurrences is set to 1
                this.Add(character, 1);
            }
            else // byte already indexed
            {
                // raise the byte's occurrence count
                this[character]++;
            }
        }

        #endregion
    }

    class HuffForest : List<HuffNode>
    {
    }

    /// <summary>
    /// The class that holds trees of Huffman nodes sorted by frequency.
    /// </summary>
    class FreqSortedHuffForest
    {
        #region Fields

        private SortedDictionary<int, HuffForest> forests;

        #endregion

        #region Properties

        /// <summary>
        /// Returns lowest frequency in the forest.
        /// </summary>
        public int LowestFreq
        {
            get { return forests.IsNullOrEmpty() ? -1 : forests.First().Key; }
        }

        /// <summary>
        /// Returns forest with the lowest frequency.
        /// </summary>
        public HuffForest ForestWithLowestFreq
        {
            get 
            {
                return
                    forests.IsNullOrEmpty() ?
                    null :
                    forests.First().Value;
            }
        }

        /// <summary>
        /// Returns the first tree with the lowest frequency in the forest.
        /// </summary>
        public HuffNode LowestTree
        {
            get
            {
                return
                    HasMoreTreesThan(0) ?
                    ForestWithLowestFreq.First() :
                    null;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new forest instance from the character frequencies
        /// dictionary.
        /// </summary>
        /// <param name="charToFreq"></param>
        public FreqSortedHuffForest(Dictionary<byte, int> charToFreq)
        {
            // initialize
            this.forests = new SortedDictionary<int, HuffForest>();

            // convert bytes with frequencies into forests of trivial huffman
            // trees, indexed by frequency
            foreach (KeyValuePair<byte, int> entry in charToFreq)
            {
                // use our own method to add new forests
                this.Add(new HuffNode(entry.Key, entry.Value, null, null));
            }

            // once we have the forests, sort them with the order defined by the
            // 'Node.CompareTo' method
            foreach (HuffForest forest in forests.Values)
            {
                forest.Sort();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds new tree into the forest.
        /// </summary>
        /// <param name="tree"></param>
        public void Add(HuffNode tree)
        {
            tree.ThrowIfNull("This program doesn't like 'null' trees.");
            if (forests.ContainsKey(tree.Frequency))
            {
                // index the new forest to the existing frequency
                forests[tree.Frequency].Add(tree);
            }
            else
            {
                // index the new frequency and forest
                forests.Add(tree.Frequency, new HuffForest() { tree });
            }
        }

        /// <summary>
        /// Removes all trees with selected frequency from the forest.
        /// </summary>
        /// <param name="frequency"></param>
        /// <returns></returns>
        public bool Remove(int frequency)
        {
            return forests.Remove(frequency);
        }

        /// <summary>
        /// Returns the if the there are more trees in the forest than the
        /// amount parameter.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool HasMoreTreesThan(int amount)
        {
            foreach(var forest in forests.Values)
            {
                amount -= forest.Count;

                if (amount < 0)
                    return true;
            }

            return false;
        }

        #endregion
    }

    /// <summary>
    /// The main program class.
    /// </summary>
    public static class Program
    {
        #region Fields

        // The number of arguments this program requires.
        private const int EXPECTED_PROGRAM_ARGS = 1;

        #endregion

        #region The program entry point

        /// <summary>
        /// The main entry point of the program.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            RunProgram(args); // run program WITHOUT stopwatch
            // ApplyStopwatch(RunProgram, args); // run program WITH stopwatch
        }

        #endregion

        #region Methods to run/benchmark the program

        private static void ApplyStopwatch(
            Action<string[]> action,
            string[] args)
        {
            // initialize stopwatch
            Stopwatch stopwatch = new Stopwatch();

            // start counting
            stopwatch.Start();

            // execute program
            action(args);

            // stop counting
            stopwatch.Stop();

            // print execution time and wait for console input to terminate the
            // process
            Console.WriteLine(string.Format(
                "Execution time: {0}mn {1}s {2}ms",
                stopwatch.Elapsed.Minutes,
                stopwatch.Elapsed.Seconds,
                stopwatch.Elapsed.TotalMilliseconds));
            Console.ReadKey();
        }

        private static void RunProgram(string[] args)
        {
            // wrong program arguments message has to be printed
            if (args.Length != EXPECTED_PROGRAM_ARGS)
            {
                Console.Write("Argument Error");
                return; // Exit the program
            }

            // if there are no problems with arguments, try to read the input
            FreqSortedHuffForest freqSortedHuffForests = null;
            try
            {
                using (var inputFileStream =
                    new FileStream(args[0], FileMode.Open, FileAccess.Read))
                {
                    // read & parse the input file
                    CharToFrequency frequencies =
                        new CharToFrequency(inputFileStream);

                    freqSortedHuffForests =
                        new FreqSortedHuffForest(frequencies);
                }
            }
            catch (Exception)
            {
                Console.Write("File Error");
                return; // Exit the program
            }

            // at this point, 'freqSortedHuffForests' must be non-null
            if (freqSortedHuffForests.HasMoreTreesThan(0))
            {
                // the condition is not necessary as the program correctly
                // handles empty inputs now
                HuffTree huffTree = new HuffTree(freqSortedHuffForests);
                huffTree.Print();
                Console.Write("\n");
            }
        }

        #endregion

        #region Extensions/conveniences

        /// <summary>
        /// Checks the source argument and throws a
        /// <see cref="NullReferenceException"/> if it is null.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public static void ThrowIfNull(this Object source, string message)
        {
            if (source == null)
            {
                throw new NullReferenceException(message);
            }
        }

        /// <summary>
        /// Returns true if the input is null or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }
            else
            {
                // Is always O(1)
                return !enumerable.Any();
            }
        }

        #endregion
    }
}
