using CWToolsHelpers.ScriptedVariables;
using NetExtensions.Collection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CWToolsHelpers.FileParsing
{
    /// <summary>
    /// A complex object in the paradox file that has many children.  Most top level items are Nodes.  
    /// </summary>
    public class CWNode
    {
        public CWNode(string key)
        {
            Key = key;
        }

        /// <summary>
        /// The node key
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// The CWNode that is the parent of this CWNode - e.g. the CWNode that contains this CWNode. 
        /// </summary>
        /// <remarks>
        /// This will be <c>null</c> for the CWNode that represents a file.  
        /// </remarks>
        public CWNode Parent { get; private set; }

        /// <summary>
        /// All child nodes of this one.
        /// </summary>
        /// <remarks>
        /// Would really like to use a dictionary here, but duplicate node keys are entirely possible, as keys are often things like logical operators
        /// </remarks>
        public IList<CWNode> Nodes
        {
            get => nodes;
            set
            {
                nodes = value;
                nodes.ForEach(node => node.Parent = this);
            }
        }

        /// <summary>
        /// All key value pairs with their raw values (e.g. no scripted variables substituted)
        /// </summary>
        /// <remarks>
        /// Would really like to use a dictionary here, but duplicate keys are entirely possible, as keys are often things like logical operators
        /// </remarks>
        public IList<CWKeyValue> RawKeyValues
        {
            get => rawKeyValues;
            set
            {
                rawKeyValues = value;
                rawKeyValues.ForEach(cwKeyValue => cwKeyValue.ParentNode = this);
            }
        }

        private IList<ICWKeyValue> keyValues;
        /// <summary>
        /// All key value pairs with their resolved values (e.g. scripted variables processed)
        /// </summary>
        /// <remarks>
        /// Would really like to use a dictionary here, but duplicate keys are entirely possible, as keys are often things like logical operators
        /// </remarks>
        public IList<ICWKeyValue> KeyValues
        {
            get { return keyValues ?? (keyValues = RawKeyValues.Select(x => (ICWKeyValue)new CWNodeContextedKeyValue(x, ScriptedVariablesAccessor)).ToList()); }
        }

        /// <summary>
        /// Straight values within the node, these are almost always comments.
        /// </summary>
        public IList<string> Values { get; set; }

        private IScriptedVariablesAccessor scriptedVariablesAccessor;
        private IList<CWNode> nodes;
        private IList<CWKeyValue> rawKeyValues;

        public IScriptedVariablesAccessor ScriptedVariablesAccessor
        {
            get => scriptedVariablesAccessor ?? (scriptedVariablesAccessor = new DummyScriptedVariablesAccessor());
            set => scriptedVariablesAccessor = value;
        }

        /// <summary>
        /// Gets the first child node with the specified key.
        /// </summary>
        /// <remarks>
        /// Use with caution, will get the first node if there are multiple with the same key in the same context!
        /// </remarks>
        /// <param name="key">They key</param>
        /// <returns></returns>
        public CWNode GetNode(string key)
        {
            return Nodes.FirstOrDefault(x => x.Key == key);
        }

        /// <summary>
        /// Get all child nodes with the specified key.
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns></returns>
        public IEnumerable<CWNode> GetNodes(string key)
        {
            return Nodes.Where(x => x.Key == key);
        }

        /// <summary>
        /// If there are any child nodes with the given key, performs the specified Action on them.
        /// </summary>
        /// <param name="key">The key of the child nodes</param>
        /// <param name="perform">The Action to perform if any are found</param>
        public void ActOnNodes(string key, Action<CWNode> perform)
        {
            ActOnNodes(key, perform, () => { });
        }

        /// <summary>
        /// If there are any child nodes with the given key, performs the specified Action on them, otherwise perform the no match action.
        /// </summary>
        /// <param name="key">The key of the child nodes</param>
        /// <param name="perform">The Action to perform if any are found</param>
        /// <param name="performIfNoMatch">The Action to perform if there is no nodes with the specified key</param>
        public void ActOnNodes(string key, Action<CWNode> perform, Action performIfNoMatch)
        {
            var nodes = GetNodes(key);
            if (nodes.Any())
            {
                foreach (var cwNode in nodes)
                {
                    perform(cwNode);
                }
            }
            else
            {
                performIfNoMatch();
            }
        }

        /// <summary>
        /// If there is a child key-value with the specified key, returns it.  Attempting to convert any variables using <see cref="ScriptedVariablesAccessor"/>
        /// </summary>
        /// <remarks>
        /// Use with caution, will get the first keyvalue if there are multiple with the same key in the same context!
        /// </remarks>
        /// <param name="key">The Key of the Keyvalue item within the node</param>
        /// <returns>See above.</returns>
        public string GetKeyValue(string key)
        {
            var value = GetRawKeyValue(key);
            return ScriptedVariablesAccessor.GetPotentialValue(value);
        }

        /// <summary>
        /// If there is a child key-value with the specified key, returns it, otherwise uses the supplied default value.  Attempting to convert any variables using <see cref="ScriptedVariablesAccessor"/>
        /// </summary>
        /// <remarks>
        /// Use with caution, will get the first keyvalue if there are multiple with the same key in the same context!
        /// </remarks>
        /// <param name="key">The Key of the Keyvalue item within the node</param>
        /// <param name="defaultValue">The value to use (and supplied to the <see cref="ScriptedVariablesAccessor"/>) if the keyvalue does not exist in the node</param>
        /// <returns>See above.</returns>
        public string GetKeyValueOrDefault(string key, object defaultValue)
        {
            var value = GetRawKeyValue(key);
            return ScriptedVariablesAccessor.GetPotentialValue(value ?? defaultValue.ToString());
        }

        /// <summary>
        /// If there are children key-value with the specified key, performs the specified action on them.  Attempting to convert any variables using <see cref="ScriptedVariablesAccessor"/>
        /// </summary>
        /// <remarks>
        /// Use with caution, will get the first keyvalue if there are multiple with the same key in the same context!
        /// </remarks>
        /// <param name="key">The Key of the Keyvalue item within the node</param>
        /// <param name="perform">The action to perform if the value exists.</param>
        /// <returns>See above.</returns>
        public void ActOnKeyValues(string key, Action<string> perform)
        {
            KeyValues.Where(x => x.Key == key).Select(x => ScriptedVariablesAccessor.GetPotentialValue(x.Value)).ForEach(perform);
        }


        /// <summary>
        /// If there is a child key-value with the specified key, returns it. Does not perform variable conversion.
        /// </summary>
        /// <remarks>
        /// Use with caution, will get the first keyvalue if there are multiple with the same key in the same context!
        /// </remarks>
        /// <param name="key">The Key of the Keyvalue item within the node</param>
        /// <returns>See above.</returns>
        public string GetRawKeyValue(string key)
        {
            return RawKeyValues.FirstOrDefault(x => x.Key == key)?.Value;
        }
    }


    public interface ICWKeyValue
    {
        string Key { get; }
        string Value { get; }
        CWNode ParentNode { get; }

        KeyValuePair<string, string> ToKeyValue();
    }

    /// <summary>
    /// A straight key = value entry in the file/node: e.g: tier = 1
    /// </summary>
    public class CWKeyValue : ICWKeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public CWNode ParentNode { get; set; }

        public KeyValuePair<string, string> ToKeyValue() { return new KeyValuePair<string, string>(Key, Value); }

        public bool Equals(KeyValuePair<string, string> obj, StringComparison comparison = StringComparison.Ordinal)
        {
            var keyValuePair = ToKeyValue();
            return keyValuePair.Key.Equals(obj.Key, comparison) && keyValuePair.Value.Equals(obj.Value, comparison);
        }
    }

    internal class CWNodeContextedKeyValue : ICWKeyValue
    {
        private readonly ICWKeyValue raw;
        private readonly IScriptedVariablesAccessor accessor;

        internal CWNodeContextedKeyValue(ICWKeyValue raw, IScriptedVariablesAccessor accessor)
        {
            this.raw = raw;
            this.accessor = accessor;
        }
        public string Key => raw.Key;
        public string Value => accessor.GetPotentialValue(raw.Value);
        public CWNode ParentNode => raw.ParentNode;

        public KeyValuePair<string, string> ToKeyValue()
        {
            return new KeyValuePair<string, string>(Key, Value);
        }

        public bool Equals(KeyValuePair<string, string> obj, StringComparison comparison = StringComparison.Ordinal)
        {
            var keyValuePair = ToKeyValue();
            return keyValuePair.Key.Equals(obj.Key, comparison) && keyValuePair.Value.Equals(obj.Value, comparison);
        }
    }
}
