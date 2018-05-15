﻿using System.Collections.Generic;

namespace Lib.AssetsPlugin
{
    public abstract class ContentBuilder
    {
        public const string Notice = "//This is AUTOGENERATED FILE by bb-assets-generator-plugin. DO NOT EDIT!!!\n";
        public const string ExportConst = "export const ";

        protected string _content;

        public void Build(IDictionary<string, object> assets)
        {
            _content = GetHeader();
            RecursiveBuild(assets, 0);
        }

        public string Content => _content;

        public abstract string GetHeader();

        public abstract void AddPropertyValue(string name, string value, int depth);

        public string GetPropertyLineEnd(int depth)
        {
            return (depth == 0 ? ";" : ",") + "\n";
        }

        protected string GetPropertyNameValueSeparator(int depth)
        {
            return depth == 0 ? " = " : ": ";
        }

        protected string SanitizePropertyName(string key)
        {
            if (key == "") return "";
            if (key[0] >= '0' && key[0] <= '9') return "_" + key;
            return key;
        }

        void RecursiveBuild(IDictionary<string, object> rootObject, int depth)
        {
            foreach (var propertyPair in rootObject)
            {
                var propertyName = propertyPair.Key;
                var propertyValue = propertyPair.Value;
                if (depth == 0)
                {
                    AddExport();
                }
                else
                {
                    AddIdent(depth);
                }

                if (propertyValue is IDictionary<string, object>)
                {
                    AddObjectStart(propertyName, depth);
                    RecursiveBuild(propertyValue as IDictionary<string, object>, depth + 1);
                    AddObjectEnd(depth);
                }
                else
                {
                    AddPropertyValue(propertyName, propertyValue as string, depth);
                }
            }
        }

        void AddObjectStart(string name, int depth)
        {
            var objectStart = SanitizePropertyName(name);
            objectStart += GetPropertyNameValueSeparator(depth);

            objectStart += "{\n";

            AddLines(objectStart);
        }

        void AddObjectEnd(int depth)
        {
            AddIdent(depth);
            AddLines("}" + (depth == 0 ? ";" : ",") + "\n");
        }

        void AddIdent(int depth)
        {
            AddLines(GetIdent(depth));
        }

        void AddExport()
        {
            AddLines(ExportConst);
        }

        void AddLines(string lines)
        {
            _content += lines;
        }

        string GetIdent(int depth)
        {
            return new string(' ', depth * 4);
        }
    }
}