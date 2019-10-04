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
        private static bool READ_ONLY = false; // default set HashMap not to be read-only

        private Node[] elements;
        private ICollection<TKey> keys;
        private ICollection<TValue> values;
        private int size;

        // Default construction: construct an empty HashMap
        public HashMap()
        {
            elements = new Node[10];
            size = 0;
            keys = new List<TKey>();
            values = new List<TValue>();
        }

        // Accessor of how many pairs of key/value in HashMap
        public int Count
        {
            get { return size; }
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

        // Get accessor of whether the HashMap is read-only or not
        public bool IsReadOnly
        {
            get { return READ_ONLY; }
        }

        // Remove all the key/value pairs in the HashMap
        public void Clear()
        {
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i] == null)
                {
                    elements[i] = null;
                }
            }
            size = 0;
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
            return size == 0;
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
            Node current;
            if (!TryGetValue(key, out value)) // add new key/value pair
            {
                current = new Node(key, value);
                current.next = elements[h];
                elements[h] = current;
                size++;
                keys.Add(key);
                values.Add(value);
            } else // update value
            {
                current = elements[h];
                while (current.next != null && !current.key.Equals(key))
                {
                    current = current.next;
                }
                current.value = value;
                values.Remove(value);
                values.Add(value);
            }
            // resize to a bigger size array if needed
            if (Convert.ToDouble(size) / elements.Length > MAX_LOAD)
            {
                Rehash();
            }
        }

        // Pre: the key passed in should not be null; otherwise, throws an NullReferencePointer.
        // Removes the given key/value pair in the HashMap if found; otherwise, returns false.
        public bool Remove(TKey key)
        {
            NullExceptionHelper(key);
            if (ContainsKey(key))
            {
                int h = Hash(key);
                if (elements[h] != null && elements[h].key.Equals(key))
                {
                    keys.Remove(key);
                    values.Remove(elements[h].value);
                    elements[h] = elements[h].next;
                    size--;
                    return true;
                } else
                {
                    Node current = elements[h];
                    while (current.next != null)
                    {
                        if (current.next.key.Equals(key))
                        {
                            keys.Remove(key);
                            values.Remove(current.next.value);
                            current.next = current.next.next;
                            size--;
                            return true;
                        }
                        current = current.next;
                    }
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
            return Math.Abs(key.GetHashCode() % elements.Length);
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
