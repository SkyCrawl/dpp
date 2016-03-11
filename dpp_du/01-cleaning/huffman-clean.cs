using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HuffmanCoding
{
	class HuffNode: IComparable<HuffNode>
	{
		private static int NODES_CREATED = 0;

		public byte character { get; private set; }
		public int frequency { get; private set; }
		public int age { get; private set; }

		public HuffNode leftSon;
		public HuffNode rightSon;

		/// <summary>
		/// Kdyz nema jedineho syna vraci true.
		/// </summary>
		/// <returns></returns>
		public bool IsLeaf
		{
			get { return (leftSon == null) && (rightSon == null); }
		}

		public HuffNode(byte character, int frequency, HuffNode leftSon, HuffNode rightSon)
		{
			this.frequency = frequency;
			this.character = character;
			this.age = NODES_CREATED++;
			this.leftSon = leftSon;
			this.rightSon = rightSon;
		}

		/// <summary>
		/// True o sobe vrchol rekne jestli bude v Huffmanskem strome nalevo od druheho vrcholu.
		/// </summary>
		/// <param name="druhy"></param>
		/// <returns></returns>
		public bool ShouldBeLeftOf(HuffNode other)
		{
			// check various properties, until we find a difference
			if(this.frequency != other.frequency)
			{
				return this.frequency < other.frequency;
			}
			else if(this.IsLeaf != other.IsLeaf)
			{
				return this.IsLeaf;
			}
			else if(this.IsLeaf && (this.character != other.character))
			{
				return this.character < other.character;
			}
			else
			{
				return this.age < other.age;
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

		#region IComparable Members

		public int CompareTo(HuffNode that)
		{
			if(this == that)
			{
				return 0;
			}
			else
			{
				return this.ShouldBeLeftOf(that) ? -1 : 1;
			}
		}

		#endregion
	}

	class HuffTree
	{
		private static byte MIN_ASCII_PRINTABLE = 32;
		private static byte MAX_ASCII_PRINTABLE = 126;

		private HuffNode root;

		private class ConstructionState
		{
			public FreqSortedHuffForests allHuffForests;
			public HuffNode leftoverForestFromLastIter;

			public bool HasTwoMoreTreesToProcess
			{
				get
				{
					return allHuffForests.HasMoreTreesThan(leftoverForestFromLastIter != null ? 0 : 1);
				}
			}

			public ConstructionState(FreqSortedHuffForests allHuffForests, HuffNode leftoverForestFromLastIter)
			{
				this.allHuffForests = allHuffForests;
				this.leftoverForestFromLastIter = leftoverForestFromLastIter;
			}
		}

		public HuffTree(FreqSortedHuffForests allHuffForests)
		{
			// create the constructor-scoped state variable, shared among related methods
			ConstructionState state = new ConstructionState(allHuffForests, null);

			// zip forests of frequency-equal characters, until there is a single forest left
			// Note: the algorithm guarantees one remaining forest, unless the input is empty
			while(state.HasTwoMoreTreesToProcess) // there are still at least two forests to zip
			{
				// get the current minimum frequency and corresponding huffman forests
				int minFreq = state.allHuffForests.LeastFreq;
				HuffForest minFreqForests = state.allHuffForests.ForestWithLeastFreq;

				// branch depending on whether we still have a leftover forest from last iteration to process
				IEnumerator<HuffNode> minFreqForestsEnumerator = minFreqForests.GetEnumerator();
				if(state.leftoverForestFromLastIter == null)
				{
					// process the current forests pair-wise, possibly leaving a leftover
					PairWiseForestZip(minFreqForestsEnumerator, state);
				}
				else // we have a leftover forest from last iteration
				{
					// if the forest happens to be empty (else branch), do nothing and remove the forest later on
					if(minFreqForestsEnumerator.MoveNext())
					{
						// then get the first currently processed forest
						HuffNode tree1 = state.leftoverForestFromLastIter;
						HuffNode tree2 = minFreqForestsEnumerator.Current; // non-null (guaranteed by condition)

						// zip the two trees into one and add it to be processed later
						ZipTrees(state, tree1, tree2);

						// reset the leftover pointer
						state.leftoverForestFromLastIter = null;

						// and now process the current forests pair-wise, possibly leaving another leftover
						PairWiseForestZip(minFreqForestsEnumerator, state);
					}
				}

				// remove the currently processed entry
				state.allHuffForests.Remove(minFreq);
			}

			// the last surviving forest is the last added forest and becomes the root
			// Note: mind empty input...
			this.root = state.allHuffForests.LeastTree; // null if empty
		}

		private void PairWiseForestZip(IEnumerator<HuffNode> enumerator, ConstructionState state)
		{
			while(enumerator.MoveNext())
			{
				HuffNode tree1 = enumerator.Current; // non-null (guaranteed by condition)

				if(enumerator.MoveNext()) // second node is available
				{
					HuffNode tree2 = enumerator.Current; // non-null (guaranteed by condition)

					// zip the two trees into one and add it to be processed later
					ZipTrees(state, tree1, tree2);
				}
				else // second node is not available
				{
					// save the first and continue iterating
					state.leftoverForestFromLastIter = tree1;
				}
			}
		}

		private void ZipTrees(ConstructionState state, HuffNode tree1, HuffNode tree2)
		{
			// create a parent node for the two nodes
			bool oneLeftOfTwo = tree1.ShouldBeLeftOf(tree2);
			HuffNode compoundTree = new HuffNode(
				tree1.character, // doesn't really matter for inner nodes because their characters are not printed out
				tree1.frequency + tree2.frequency,
				oneLeftOfTwo ? tree1 : tree2, // one is left
				oneLeftOfTwo ? tree2 : tree1); // and the other is right

			// and add it to be processed later
			state.allHuffForests.Add(compoundTree);
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

		public void Print()
		{
			if(this.root != null)
			{
				PrintSubtree(this.root, string.Empty);
			}
		}

		private void PrintSubtree(HuffNode subtreeRoot, string indentation)
		{
			if(subtreeRoot.IsLeaf)
			{
				// print self
				if((subtreeRoot.character < MIN_ASCII_PRINTABLE) || (subtreeRoot.character > MAX_ASCII_PRINTABLE))
				{
					Console.WriteLine(" [{0}:{1}]", subtreeRoot.character, subtreeRoot.frequency);
				}
				else
				{
					Console.WriteLine(" ['{0}':{1}]", (char) subtreeRoot.character, subtreeRoot.frequency);
				}
			}
			else
			{
				// first raise indentation
				indentation = indentation + new string(' ', 6);

				// then print self
				Console.Write("{0,4} -+- ", subtreeRoot.frequency);

				// and finally, print the subtree
				PrintSubtree(subtreeRoot.rightSon, indentation + "|  ");
				Console.Write("{0}|\n{0}`- ", indentation);
				PrintSubtree(subtreeRoot.leftSon, indentation + new string(' ', 3));
			}
		}
	}

	class CharToFrequency : Dictionary<byte, int>
	{
		private static int BUFF_SIZE = 0x4000; // 16384

		public CharToFrequency(FileStream inputFileStream)
		{
			// read and process the file by chunks
			int bytesRead = 0;
			do
			{
				// declare buffer
				byte[] buff = new byte[BUFF_SIZE];

				// read
				bytesRead = inputFileStream.Read(buff, 0, BUFF_SIZE);

				// index each byte read
				for(int i = 0; i < bytesRead; i++)
				{
					if(!this.ContainsKey(buff[i])) // byte not yet indexed
					{
						// index it, number of occurrences is set to 1
						this.Add(buff[i], 1);
					}
					else // byte already indexed
					{
						// raise the byte's occurrence count
						this[buff[i]]++;
					}
				}
			}
			while(bytesRead == BUFF_SIZE);
		}
	}

	class HuffForest : List<HuffNode>
	{
	}

	class FreqSortedHuffForests
	{
		private SortedDictionary<int, HuffForest> forests;

		public int LeastFreq
		{
			get { return forests.IsNullOrEmpty() ? -1 : forests.Keys.Min(); }
		}

		public HuffForest ForestWithLeastFreq
		{
			get { return forests.IsNullOrEmpty() ? null : forests[LeastFreq]; }
		}

		public HuffNode LeastTree
		{
			get { return HasMoreTreesThan(0) ? ForestWithLeastFreq.First() : null; }
		}

		public FreqSortedHuffForests(CharToFrequency charToFreq)
		{
			// initialize
			this.forests = new SortedDictionary<int, HuffForest>();

			// convert bytes with frequencies into forests of trivial huffman trees, indexed by frequency
			foreach(KeyValuePair<byte, int> entry in charToFreq)
			{
				// use our own method to add new forests
				this.Add(new HuffNode(entry.Key, entry.Value, null, null));
			}

			// once we have the forests, sort them with the order defined by the 'Node.CompareTo' method
			foreach(HuffForest forest in forests.Values)
			{
				forest.Sort();
			}
		}

		public void Add(HuffNode tree)
		{
			tree.ThrowIfNull("This program doesn't like 'null' trees.");
			if(forests.ContainsKey(tree.frequency))
			{
				// index the new forest to the existing frequency
				forests[tree.frequency].Add(tree);
			}
			else
			{
				// index the new frequency and forest
				forests.Add(tree.frequency, new HuffForest() { tree });
			}
		}

		public bool Remove(int frequency)
		{
			return forests.Remove(frequency);
		}

		public bool HasMoreTreesThan(int amount)
		{
			IEnumerator<int> freqEnumerator = forests.Keys.GetEnumerator();
			int treesCounted = 0;
			while(freqEnumerator.MoveNext() && (treesCounted <= amount))
			{
				treesCounted += forests[freqEnumerator.Current].Count;
			}
			return treesCounted > amount;
		}
	}

	public static class Program
	{
		// The number of arguments this program requires.
		private const int EXPECTED_PROGRAM_ARGS = 1;

		public static void Main(string[] args)
		{
			RunProgram(args); // run program WITHOUT stopwatch
			// ApplyStopwatch(RunProgram, args); // run program WITH stopwatch
		}

		#region Methods to run/benchmark the program

		private static void ApplyStopwatch(Action<string[]> action, string[] args)
		{
			// initialize stopwatch
			Stopwatch stopwatch = new Stopwatch();

			// start counting
			stopwatch.Start();

			// execute program
			action(args);

			// stop counting
			stopwatch.Stop();

			// print execution time and wait for console input to terminate the process
			Console.WriteLine(string.Format(
				"Execution time: {0}mn {1}s {2}ms",
				stopwatch.Elapsed.Minutes,
				stopwatch.Elapsed.Seconds,
				stopwatch.Elapsed.TotalMilliseconds));
			Console.ReadKey();
		}

		private static void RunProgram(string[] args)
		{
			// wrong usage has to be printed and correct usage explained
			if(args.Length != EXPECTED_PROGRAM_ARGS)
			{
				PrintUsage(args);
				return; // required
			}

			// if there are no problems with arguments, try to read the input
			FreqSortedHuffForests freqSortedHuffForests = null;
			try
			{
				using(var inputFileStream = new FileStream(args[0], FileMode.Open, FileAccess.Read))
				{
					// read & parse the input file
					freqSortedHuffForests = new FreqSortedHuffForests(new CharToFrequency(inputFileStream));
				}
			}
			catch(Exception e)
			{
				Console.WriteLine("Error: could not read the input file.");
				Console.WriteLine(e.ToString());
				return; // required
			}

			// at this point, 'freqSortedHuffForests' must be non-null
			if(freqSortedHuffForests.HasMoreTreesThan(0))
			{
				// the condition is not necessary as the program correctly handles empty inputs now
				HuffTree huffTree = new HuffTree(freqSortedHuffForests);
				huffTree.Print();
				Console.WriteLine();
			}
		}

		#endregion

		private static void PrintUsage(string[] args)
		{
			Console.WriteLine(string.Format(
				"Error: {0} argument(s) found. Expected: {1}",
				args.Length,
				EXPECTED_PROGRAM_ARGS));
			Console.WriteLine("\tFirst argument needs to be a path to the input file.");
		}

		#region Our own extensions/conveniences

		public static void ThrowIfNull(this Object source, string message)
		{
			if(source == null)
			{
				throw new System.NullReferenceException(message);
			}
		}

		public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
		{
			if(enumerable == null)
			{
				return true;
			}
			else
			{
				var collection = enumerable as ICollection<T>;
				if(collection != null) // cast successful
				{
					// a little performance tweak: O(1)
					return collection.Count == 0;
				}
				else // cast unsuccessful
				{
					// generic answer: O(n)
					return !enumerable.Any();
				}
			}
		}

		#endregion
	}
}
