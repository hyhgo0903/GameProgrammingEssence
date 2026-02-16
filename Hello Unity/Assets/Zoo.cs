using UnityEngine;

public class Zoo : MonoBehaviour
{
  public void Start()
  {
    var tom = new Animal();
    tom.name = "Åè";
    tom.sound = "¾ß¿Ë";

    tom.PlaySound();
  }
}
