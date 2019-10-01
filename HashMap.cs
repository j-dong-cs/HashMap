using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashMap
{
    // A HashMap object represents a set of pairs of keys mapping to values 
    // using Dictionary as internal structure.
    public class HashMap<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private static double MAX_LOAD = 0.75; // load factor for rehashing

        private Node[] elements;
        private int size;

        // Default construction: construct an empty HashMap
        public HashMap()
        {
            elements = new Node[10];
            size = 0;
        }

        // Remove all the key/value pairs in the hashmap
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
            } else // update value
            {
                current = elements[h];
                while (current.next != null && !current.key.Equals(key))
                {
                    current = current.next;
                }
                current.value = value;
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
                    elements[h] = elements[h].next;
                }
                return true;
            }
            return false;
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
            }
        }

    }

}
