using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float height;

    private Vector3 startPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position; //posicion inicial en el juego
    }

    // Update is called once per frame
    void Update()
    {
        //Calcular el espacio restante por recorrer para un ciclo en base a la distancia d=vt
        float resto = (speed * Time.time) % height;
        //verificar la posicion, al cumplirse un ciclo el mismo se reinicia.
        transform.position = startPosition + resto * direction;
    }
}
