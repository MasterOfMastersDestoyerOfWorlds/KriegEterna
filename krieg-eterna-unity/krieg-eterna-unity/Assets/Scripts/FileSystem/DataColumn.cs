using System.Collections;
using System;
using System.Runtime;
using System.Collections.Generic;
using UnityEngine;


public interface IDataColumn
{
    string columnName { get; set; }

    int columnIdx { get; set; }
    bool optional { get; set; }
    void AddOptional(string[] temp, int lineNumber);

    public bool Equals(object c);
}
public class DataColumn<T> : List<T>, IDataColumn
{
    public string columnName { get; set; }
    public int columnIdx { get; set; }
    public bool optional { get; set; }
    public DataColumn(string columnName, bool optional = true)
    {
        this.columnName = columnName;
        this.optional = optional;

        if (CardModel.columns.Find((x) => x.columnName == System.Text.RegularExpressions.Regex.Replace(columnName, @"\s+", "")) == null)
        {
            CardModel.columns.Add(this);
        }
    }

    public void AddOptional(string[] temp, int lineNumber)
    {
        if (!System.String.IsNullOrEmpty(temp[this.columnIdx]))
        {
            if (typeof(T) == typeof(int))
            {
                this.Add((T)(object)int.Parse(temp[this.columnIdx]));
            }
            else if (typeof(T) == typeof(float))
            {
                this.Add((T)(object)float.Parse(temp[this.columnIdx]));
            }
            else if (typeof(T) == typeof(bool))
            {
                this.Add((T)(object)bool.Parse(temp[this.columnIdx]));
            }
            else if (typeof(T) == typeof(string))
            {
                Debug.Log("String: " + temp[this.columnIdx]);
                this.Add((T)(object)temp[this.columnIdx]);
            }
            else
            {
                this.Add((T)System.Enum.Parse(typeof(T), temp[this.columnIdx], true));

            }

        }
        else
        {

            if (optional)
            {
                this.Add(default(T));
            }
            else
            {
                Debug.LogError("Non-optional field " + columnName + " not found at column: " + this.columnIdx + " row: " + lineNumber);
            }
        }
    }

    public override string ToString()
    {
        return columnName + " optional: " + optional + " idx: " + columnIdx ;
    }
}