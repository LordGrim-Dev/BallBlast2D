
using UnityEngine;

public interface IPoolMember
{
    bool IsInUse { get; }

    int ID { get; }

    // Function to enable object
    void Show();
    void Hide();

    //Called when object is been created
    void OnCreated();
}