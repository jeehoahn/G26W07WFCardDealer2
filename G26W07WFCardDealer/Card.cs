using System;

namespace G26W07WFCardDealer {
    public enum Suit {
        Spades,
        Hearts,
        Diamonds,
        Clubs
    }

    public class Card : IComparable<Card> {
        public Suit Suit { get; set; }
        public int Rank { get; set; } // 2 ~ 14 (14 = Ace)

        public Card(Suit suit, int rank) {
            Suit = suit;
            Rank = rank;
        }

        // 0~51 범위의 정수(value)를 Card 객체로 변환하는 정적(Static) 팩토리 메서드
        public static Card FromValue(int value) {
            // value / 13 => 0: Spades, 1: Hearts, 2: Diamonds, 3: Clubs
            Suit suit = (Suit)(value / 13);

            // value % 13 => 0~12 (0: Ace -> 14, 1~12 -> 2~13)
            int rawRank = value % 13;
            int rank = (rawRank == 0) ? 14 : rawRank + 1; // Ace를 가장 높은 14로 설정

            return new Card(suit, rank);
        }

        // 리소스에 등록된 이미지 이름 변환 로직
        public string GetImageName() {
            string[] suitNames = ["spades", "hearts", "diamonds", "clubs"];
            string suitStr = suitNames[(int)Suit];

            string rankStr = Rank switch {
                14 => "ace",
                11 => "jack",
                12 => "queen",
                13 => "king",
                _ => Rank.ToString()
            };

            // J, Q, K는 뒤에 '2'가 붙는 규칙 적용
            if (rankStr is "jack" or "queen" or "king") {
                return $"{rankStr}_of_{suitStr}2";
            }
            return $"{rankStr}_of_{suitStr}";
        }

        // 기본 정렬: 숫자(Rank) 내림차순
        public int CompareTo(Card other) {
            if (other == null) return 1;
            return other.Rank.CompareTo(this.Rank);
        }
    }
}