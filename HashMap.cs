using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashMap
{
    // A HashMap object represents a set of pairs of keys mapping to values 
    // using Dictionary as internal structure.
    public class HashMap<TKey, TValue>
    {
        private static double MAX_LOAD = 0.75; // load factor for rehashing
        private static int DEFAULT_LENGTH = 10; // default LENGTH for HashMap

        private Node[] elements;
        private ICollection<TKey> keys;
        private ICollection<TValue> values;
        private int count;

        // Default construction: construct an empty HashMap
        public HashMap()
        {
            elements = new Node[DEFAULT_LENGTH];
            count = 0;
            keys = new List<TKey>();
            values = new List<TValue>();
        }

        // Accessor of how many pairs of key/value in HashMap
        public int Count
        {
            get { return count; }
        }

        // Get accessor of retrieving all keys in HashMap
        public ICollection<TKey> Keys
        {
            get { return keys; }
        }

        // Get accessor of retrieving all values in HashMap
        public ICollection<TValue> Values
        {
            get { return values; }
        }

        // Get and set accessor of the value corresponding to given key 
        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (TryGetValue(key, out value))
                {
                    return value;
                } else
                {
                    return default(TValue);
                }
            }

            set
            {
                this.Add(key, value);
            }
        }

        // Remove all the key/value pairs in the HashMap
        public void Clear()
        {
            for (int i = 0; i < elements.Length; i++)
            {
                elements[i] = null;
            }
            count = 0;
            keys.Clear();
            values.Clear();
        }

        // Pre: The key passed in shoule not be null; otherwise, throws an NullReferenceException
        // Returns true if HashMap contains a key/value pair that includes the passed key. 
        // If not, returns false.
        public bool ContainsKey(TKey key)
        {
            TValue value;
            return TryGetValue(key, out value);
        }

        // Pre: the key passed in should not be null; otherwise, throws an NullReferenceException
        // Returns true and the value associted with given key; if the given key does not exist in the
        // HashMap, returns false and the default TValue value
        public bool TryGetValue(TKey key, out TValue value)
        {
            NullExceptionHelper(key);
            int h = Hash(key);
            Node current = elements[h];
            while (current != null)
            {
                if (current.key.Equals(key))
                {
                    value = current.value;
                    return true;
                }
                current = current.next;
            }
            value = default(TValue);
            return false;
        }


        // Returns true if the HashMap does not have any key/value pair; otherwise, return false.
        public bool IsEmpty()
        {
            return count == 0;
        }
        

        // Pre: the key and value passed in should not be null; otherwise, throw an NullReferenceException
        // Adds the given key/value pair in the HashMap. If existing key already in the HashMap, then update 
        // the value corresponding to the key
        public void Add(TKey key, TValue value)
        {
            if (key == null || value == null)
            {
                throw new NullReferenceException();
            }
            int h = Hash(key);
            Node newNode = new Node(key, value);
            Node current = elements[h];
            while (current != null)
            {   
                if (current.key.Equals(key)) // update existing key's value
                {
                    current.value = value;
                    return; // stop after updating the value corresponding to given key
                }
                current = current.next;
            }
            // no existing key, add newNode to the front
            newNode.next = elements[h];
            elements[h] = newNode;
            count++;

            // resize to a bigger size array if needed
            if (Convert.ToDouble(count) / elements.Length > MAX_LOAD) Rehash();
        }

        // Pre: the key passed in should not be null; otherwise, throws an NullReferencePointer.
        // Removes the given key/value pair in the HashMap if found; otherwise, returns false.
        public bool Remove(TKey key)
        {
            NullExceptionHelper(key);
            int h = Hash(key);
            if (elements[h] == null) return false;
            // remove the front node
            if (elements[h].key.Equals(key))
            {
                elements[h] = elements[h].next;
                count--;
                return true;
            }
            else // check the rest list
            {
                Node current = elements[h];
                while (current.next != null)
                {
                    if (current.next.key.Equals(key))
                    {
                        current.next = current.next.next;
                        count--;
                        return true;
                    }
                    current = current.next;
                }
            }
            return false;
        }

        // Returns a string representation of the HashMap's elements
        override
        public string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i] != null)
                {
                    Node current = elements[i];
                    while (current.next != null)
                    {
                        sb.Append(current.key);
                        sb.Append("=");
                        sb.Append(current.value);
                        sb.Append(", ");
                        current = current.next;
                    }
                    sb.Append(current.key);
                    sb.Append("=");
                    sb.Append(current.value);
                    sb.Append(", ");
                }
            }
            sb.Append("}");
            return sb.ToString();
        }        

        // The method represent a hash funtion which maps key to corresponding index
        private int Hash(TKey key)
        {
            return Math.Abs(key.GetHashCode()) % elements.Length;
        }
        

        // Resized the hash table to be as twice size as its original size
        private void Rehash()
        {
            Node[] twoTimesElements = new Node[2 * elements.Length];
            Node[] temp = elements;
            this.elements = twoTimesElements;
            foreach (Node index in temp)
            {
                Node current = index;
                while (current != null)
                {
                    Add(current.key, current.value);
                    current = current.next;
                }
            }
        }

        // This helper method throws a NullReferenceException if the key passied is null
        private void NullExceptionHelper(TKey key)
        {
            if (key == null)
            {
                throw new NullReferenceException();
            }
        }

        // internal nested Node class has key/value pair and one pointer points to next Node
        private class Node
        {
            public TKey key;
            public TValue value;
            public Node next;

            public Node(TKey key, TValue value)
            {
                this.key = key;
                this.value = value;
                next = null;
            }
        }

    }

}
