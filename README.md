# Pong

Esse � um clone do jogo Pong feito em C# com a engine [MonoGame](https://monogame.net/).

![Foto do jogo em seu estado inicial](../../docs/cover.png)

## Intelig�ncia Artificial do Inimigo

Calculamos o ponto de intersec��o do Paddle com a bola, assim sabemos para onde ir.

![Representa��o do ponto de intersec��o](../../docs/interseccao.png)

O ponto de interse��o $(x, y)$ entre dois segmentos de linha $AB$ e $CD$ pode ser encontrado usando as seguintes f�rmulas:

\[
\text{denominador} = (x_A - x_B) \times (y_C - y_D) - (y_A - y_B) \times (x_C - x_D)
\]

Se $\text{denominador} = 0$, as linhas s�o paralelas e n�o h� ponto de interse��o.

Caso contr�rio, calcule:

\[
t = \frac{(x_A - x_C) \times (y_C - y_D) - (y_A - y_C) \times (x_C - x_D)}{\text{denominador}}
\]

\[
u = -\frac{(x_A - x_B) \times (y_A - y_C) - (y_A - y_B) \times (x_A - x_C)}{\text{denominador}}
\]

Se $t$ e $u$ estiverem ambos no intervalo $[0, 1]$, ent�o o ponto de interse��o est� dentro dos segmentos de linha, e suas coordenadas s�o:

\[
x = x_A + t \times (x_B - x_A)
\]

\[
y = y_A + t \times (y_B - y_A)
\]

A partir daqui, mudamos o tempo de rea��o do inimigo de acordo com suas vit�rias e derrotas. Se estiver ganhando muito, aumentamos o tempo de rea��o, facilitando o jogo, se estiver ganhando pouco diminuimos o tempo de rea��o, dificultando o jogo. Com isso temos um jogo que permanece balanceado.

## Cr�ditos

- Arte por [Esoe B.Studios](https://myebstudios.itch.io/)
- Efeitos sonoros por [NoiseCollector](https://freesound.org/people/NoiseCollector/)
