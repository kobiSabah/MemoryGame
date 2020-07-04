using System;

// ReSharper disable once CheckNamespace
namespace MemoryGameUI
{
    public struct Card
    {
        private readonly char r_Value;

        private Card(char i_Value)
        {
            r_Value = i_Value;
        }

        public static Card[] CreatePairsDeck(int i_NumberOfCards)
        {
            Card[] deckOfCards = new Card[i_NumberOfCards];
            char cardValue = 'A';
            for (int i = 0; i < deckOfCards.Length; i += 2)
            {
                deckOfCards[i] = new Card(cardValue);
                deckOfCards[i + 1] = deckOfCards[i];
                cardValue++;
            }

            shuffleDeck(deckOfCards);

            return deckOfCards;
        }

        public string Value
        {
            get
            {
                return r_Value.ToString();
            }
        }

        private static void shuffleDeck(Card[] i_DeckOfCards)
        {
            Random random = new Random();

            for (int i = 0; i < i_DeckOfCards.Length; i++)
            {
                int randomIndex = random.Next(0, i_DeckOfCards.Length);

                swap(ref i_DeckOfCards[i], ref i_DeckOfCards[randomIndex]);
            }
        }

        private static void swap(ref Card i_Card1, ref Card i_Card2)
        {
            Card temp = i_Card1;

            i_Card1 = i_Card2;
            i_Card2 = temp;
        }
    }
}
