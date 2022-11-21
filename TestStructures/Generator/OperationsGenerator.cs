using TestStructures.Generator.Helpers;

namespace TestStructures.Generator
{
    internal class OperationsGenerator
    {
        public Counter Add { get; set; }
        public Counter Remove { get; set; }
        public Counter Get { get; set; }
        public List<int> KeysForOperations { get; set; }
        public Random Generator { get; set; }

        public OperationsGenerator(int insert, int get, int remove, int? seed = null)
        {
            Add = new Counter(insert);
            Remove = new Counter(get);
            Get = new Counter(remove);
            KeysForOperations = new List<int>();
            Generator = seed == null ? new Random(seed!.Value) : new Random();
        }

        public Operations GenerateOperation()
        {
            int randomNumber;
            Operations? operation = null;
            if (!Add.IsCountReached() && !Remove.IsCountReached() && !Get.IsCountReached())
            {
                randomNumber = Generator.Next(0, 3);
                if (randomNumber == 0 || Add.CurrentCount == Remove.CurrentCount)
                {
                    operation = AddOrRemove();
                }
                else
                {
                    operation = Operations.Get;

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
                    operation = Operations.Add;
                }
                else
                {
                    operation = Operations.Get;
                }
            }

            else if (Add.IsCountReached() && !Remove.IsCountReached() && !Get.IsCountReached())
            {
                randomNumber = Generator.Next(0, 2);
                if (randomNumber == 0)
                {
                    operation = Operations.Remove;
                }
                else
                {
                    operation = Operations.Get;

                }
            }

            else if (!Add.IsCountReached() && Remove.IsCountReached() && Get.IsCountReached())
            {
                operation = Operations.Add;
            }

            else if (Add.IsCountReached() && !Remove.IsCountReached() && Get.IsCountReached())
            {
                operation = Operations.Remove;
            }

            else if (Add.IsCountReached() && Remove.IsCountReached() && !Get.IsCountReached())
            {
                operation = Operations.Get;
            }

            else if (operation == null)
            {
                throw new InvalidOperationException("Invalid operation generated");
            }

            switch (operation)
            {
                case Operations.Add:
                    Add.Increment();
                    break;
                case Operations.Remove:
                    Remove.Increment();
                    break;
                case Operations.Get:
                    Get.Increment();
                    break;
            }

            return (Operations)operation;
        }

        public int GenerateKey(Operations operation)
        {
            int key;
            switch (operation)
            {
                case Operations.Add:
                    key = Generator.Next();
                    KeysForOperations.Add(key);
                    return key;
                    case Operations.Get:
                    key = KeysForOperations[Generator.Next(0, KeysForOperations.Count)];
                    return key;
                    case Operations.Remove:
                    key = KeysForOperations[Generator.Next(0, KeysForOperations.Count)];
                    KeysForOperations.Remove(key);
                    return key;
                    
            }

            return -1;
        }

        private Operations AddOrRemove()
        {
            if (Add.CurrentCount == Remove.CurrentCount)
            {
                return Operations.Add;
            }
            else if (!Add.IsCountReached() && !Remove.IsCountReached())
            {
                int rand = Generator.Next(0, 2);
                if (rand == 0)
                {
                    return Operations.Add;
                }

                return Operations.Remove;
            }
            else if (Add.IsCountReached() && !Remove.IsCountReached())
            {
                return Operations.Remove;
            }
            else if (!Add.IsCountReached() && Remove.IsCountReached())
            {
                return Operations.Add;
            }

            throw new InvalidOperationException("Unable to choose operation");
        }

        public void AddOperation(int index, Operations operation, List<int> actualKeys)
        {
            int key;
            if (operation == Operations.Add)
            {
                key = Generator.Next();
                KeysForOperations.Add(key);
                actualKeys.Add(key);
            }
            else if (operation == Operations.Get)
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

                case Operations.Add:
                    {
                        Add.Increment();
                        break;
                    }
                case Operations.Remove:
                    {
                        Remove.Increment();
                        break;
                    }
                case Operations.Get:
                    {
                        Get.Increment();
                        break;
                    }
            }
        }

    }
}
