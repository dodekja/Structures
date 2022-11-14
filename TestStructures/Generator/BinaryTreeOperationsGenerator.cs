using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStructures.Generator.Helpers;

namespace TestStructures.Generator
{
    internal class TreeOperationsGenerator
    {
        public Counter Add { get; set; }
        public Counter Remove { get; set; }
        public Counter Get { get; set; }
        public List<int> KeysForOperations { get; set; }
        public Random Generator { get; set; }

        public TreeOperationsGenerator(int insert, int get, int remove, int? seed = null)
        {
            Add = new Counter(insert);
            Remove = new Counter(get);
            Get = new Counter(remove);
            KeysForOperations = new List<int>();
            Generator = seed == null ? new Random(seed!.Value) : new Random();
        }

        public TreeOperations GenerateOperation()
        {
            int randomNumber;
            TreeOperations? operation = null;
            if (!Add.IsCountReached() && !Remove.IsCountReached() && !Get.IsCountReached())
            {
                randomNumber = Generator.Next(0, 3);
                if (randomNumber == 0 || Add.CurrentCount == Remove.CurrentCount)
                {
                    operation = AddOrRemove();
                }
                else
                {
                    operation = TreeOperations.Get;

                }
            }

            else if (!Add.IsCountReached() && !Remove.IsCountReached() && Get.IsCountReached())
            {
                operation = AddOrRemove();
            }

            else if (!Add.IsCountReached() && Remove.IsCountReached() && !Get.IsCountReached())
            {
                randomNumber = Generator.Next(0, 2);
                if (randomNumber == 0)
                {
                    operation = TreeOperations.Add;
                }
                else
                {
                    operation = TreeOperations.Get;
                }
            }

            else if (Add.IsCountReached() && !Remove.IsCountReached() && !Get.IsCountReached())
            {
                randomNumber = Generator.Next(0, 2);
                if (randomNumber == 0)
                {
                    operation = TreeOperations.Remove;
                }
                else
                {
                    operation = TreeOperations.Get;

                }
            }

            else if (!Add.IsCountReached() && Remove.IsCountReached() && Get.IsCountReached())
            {
                operation = TreeOperations.Add;
            }

            else if (Add.IsCountReached() && !Remove.IsCountReached() && Get.IsCountReached())
            {
                operation = TreeOperations.Remove;
            }

            else if (Add.IsCountReached() && Remove.IsCountReached() && !Get.IsCountReached())
            {
                operation = TreeOperations.Get;
            }

            else if (operation == null)
            {
                throw new InvalidOperationException("Invalid operation generated");
            }

            switch (operation)
            {
                case TreeOperations.Add:
                    Add.Increment();
                    break;
                case TreeOperations.Remove:
                    Remove.Increment();
                    break;
                case TreeOperations.Get:
                    Get.Increment();
                    break;
            }

            return (TreeOperations)operation;
        }

        public int GenerateKey(TreeOperations operation)
        {
            int key;
            switch (operation)
            {
                case TreeOperations.Add:
                    key = Generator.Next();
                    KeysForOperations.Add(key);
                    return key;
                    case TreeOperations.Get:
                    key = KeysForOperations[Generator.Next(0, KeysForOperations.Count)];
                    return key;
                    case TreeOperations.Remove:
                    key = KeysForOperations[Generator.Next(0, KeysForOperations.Count)];
                    KeysForOperations.Remove(key);
                    return key;
                    
            }

            return -1;
        }

        private TreeOperations AddOrRemove()
        {
            if (Add.CurrentCount == Remove.CurrentCount)
            {
                return TreeOperations.Add;
            }
            else if (!Add.IsCountReached() && !Remove.IsCountReached())
            {
                int rand = Generator.Next(0, 2);
                if (rand == 0)
                {
                    return TreeOperations.Add;
                }

                return TreeOperations.Remove;
            }
            else if (Add.IsCountReached() && !Remove.IsCountReached())
            {
                return TreeOperations.Remove;
            }
            else if (!Add.IsCountReached() && Remove.IsCountReached())
            {
                return TreeOperations.Add;
            }

            throw new InvalidOperationException("Unable to choose operation");
        }

        public void AddOperation(int index, TreeOperations operation, List<int> actualKeys)
        {
            int key;
            if (operation == TreeOperations.Add)
            {
                key = Generator.Next();
                KeysForOperations.Add(key);
                actualKeys.Add(key);
            }
            else if (operation == TreeOperations.Get)
            {
                key = Generator.Next(0, actualKeys.Count);
                KeysForOperations.Add(actualKeys[key]);
            }
            else
            {
                int actualKeysIndex = Generator.Next(0, actualKeys.Count);
                key = actualKeys[actualKeysIndex];
                KeysForOperations.Add(key);
                actualKeys.Remove(key);
            }

            switch (operation)
            {

                case TreeOperations.Add:
                    {
                        Add.Increment();
                        break;
                    }
                case TreeOperations.Remove:
                    {
                        Remove.Increment();
                        break;
                    }
                case TreeOperations.Get:
                    {
                        Get.Increment();
                        break;
                    }
            }
        }

    }
}
