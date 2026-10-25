using System;
using System.Collections.Generic;
using System.Linq;

namespace G26W07WFCardDealer {
    // 족보 종류 열거형 (점수 순서대로 배치)
    public enum HandRank {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        Straight,
        Flush,
        FullHouse,
        FourOfAKind,
        StraightFlush,
        RoyalFlush
    }

    // 판별 결과를 담는 클래스
    public class EvaluationResult {
        public HandRank Rank { get; set; }
        public string HandName { get; set; }
        public List<Card> SortedCards { get; set; }

        public EvaluationResult(HandRank rank, string handName, List<Card> sortedCards) {
            Rank = rank;
            HandName = handName;
            SortedCards = sortedCards;
        }
    }

    public static class PokerEvaluator {
        public static EvaluationResult Evaluate(List<Card> cards) {
            if (cards == null || cards.Count != 5) {
                throw new ArgumentException("카드는 정확히 5장이어야 합니다.");
            }

            // 1. 숫자의 빈도수 계산 (예: [10, 10, 10, 2, 2] -> 10이 3개, 2가 2개)
            var rankCounts = cards.GroupBy(c => c.Rank)
                                  .OrderByDescending(g => g.Count())
                                  .ThenByDescending(g => g.Key)
                                  .ToList();

            // 2. 족보에 맞게 카드 재정렬 (페어/트리플인 카드가 앞으로 오도록 정렬)
            List<Card> sortedCards = new List<Card>();
            foreach (var group in rankCounts) {
                sortedCards.AddRange(cards.Where(c => c.Rank == group.Key).OrderByDescending(c => c.Rank));
            }

            // 3. 플러시 및 스트레이트 여부 확인
            bool isFlush = cards.Select(c => c.Suit).Distinct().Count() == 1;

            var sortedRanks = cards.Select(c => c.Rank).OrderByDescending(r => r).ToList();
            bool isStraight = false;

            // 일반 스트레이트 (예: 14, 13, 12, 11, 10)
            if (sortedRanks[0] - sortedRanks[4] == 4 && sortedRanks.Distinct().Count() == 5) {
                isStraight = true;
            }
            // 백스트레이트 예외 처리 (A, 5, 4, 3, 2)
            else if (sortedRanks.SequenceEqual(new List<int> { 14, 5, 4, 3, 2 })) {
                isStraight = true;
            }

            // 4. 족보 조건 판별 (높은 족보부터 검사)
            if (isFlush && isStraight) {
                if (sortedRanks[0] == 14 && sortedRanks[1] == 13)
                    return new EvaluationResult(HandRank.RoyalFlush, "로열 플러시 (Royal Flush)", sortedCards);
                return new EvaluationResult(HandRank.StraightFlush, "스트레이트 플러시 (Straight Flush)", sortedCards);
            }

            if (rankCounts[0].Count() == 4)
                return new EvaluationResult(HandRank.FourOfAKind, "포카드 (Four of a Kind)", sortedCards);

            if (rankCounts[0].Count() == 3 && rankCounts[1].Count() == 2)
                return new EvaluationResult(HandRank.FullHouse, "풀하우스 (Full House)", sortedCards);

            if (isFlush)
                return new EvaluationResult(HandRank.Flush, "플러시 (Flush)", sortedCards);

            if (isStraight)
                return new EvaluationResult(HandRank.Straight, "스트레이트 (Straight)", sortedCards);

            if (rankCounts[0].Count() == 3)
                return new EvaluationResult(HandRank.ThreeOfAKind, "트리플 (Three of a Kind)", sortedCards);

            if (rankCounts[0].Count() == 2 && rankCounts[1].Count() == 2)
                return new EvaluationResult(HandRank.TwoPair, "투페어 (Two Pair)", sortedCards);

            if (rankCounts[0].Count() == 2)
                return new EvaluationResult(HandRank.OnePair, "원페어 (One Pair)", sortedCards);

            return new EvaluationResult(HandRank.HighCard, "하이카드 (High Card)", sortedCards);
        }
    }
}