using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


public interface IDataColumn
{
    string columnName { get; set; }

    int columnIdx { get; set; }
    void AddOptional(string[] temp);
}
public class DataColumn<T> : List<T>, IDataColumn {
    public string columnName { get; set; }
    public int columnIdx { get; set; }
    public DataColumn(string columnName){
        this.columnName = columnName;
        CardModel.columns.Add(this);
    }

    public void AddOptional(string[] temp)
    {
        if (!System.String.IsNullOrEmpty(temp[this.columnIdx]))
        {
            if(typeof(T) == typeof(int)){
                this.Add((T)(object)int.Parse(temp[this.columnIdx]));
            }else if(typeof(T) == typeof(bool)){
                this.Add((T)(object)bool.Parse(temp[this.columnIdx]));
            }else if(typeof(T) == typeof(string)){
                this.Add((T)(object)temp[this.columnIdx]);
            }else{
                this.Add((T)System.Enum.Parse(typeof(T), temp[this.columnIdx], true));
            }
            
        }
        else
        {
            this.Add(default(T));
        }
    }
}