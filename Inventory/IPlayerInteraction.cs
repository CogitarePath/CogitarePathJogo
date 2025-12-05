using UnityEngine;

public interface IPlayerInteraction
{
    bool Interaction(); // método genérico para todas as interações

    string InteractionMessage(bool completedInteraction); // mensagem quando o jogador aperta R

    public string LookMessage(); //mensagem na tela quando o jogador olha
}
