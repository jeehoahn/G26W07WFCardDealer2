using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace G26W07WFCardDealer {
    public partial class Form1 : Form {

        public Form1() {
            InitializeComponent();
        }

        private void OnDeal(object sender, EventArgs e) {
            int[] card_value = new int[52];
            Random random = new Random();
            List<Card> drawnCards = new List<Card>();

            // 1. 중복 없이 5장의 숫자(0~51)를 뽑고 Card 객체로 생성
            for (int i = 0; i < 5; ++i) {
                int value;
                while (true) {
                    value = random.Next(52);
                    if (card_value[value] == 0) {
                        break;
                    }
                }
                card_value[value] = 1;

                // Card 객체로 변환하여 리스트에 추가
                drawnCards.Add(Card.FromValue(value));
            }

            // 2. 비즈니스 로직(PokerEvaluator) 호출 -> 족보 판별 및 정렬된 카드 수령
            EvaluationResult result = PokerEvaluator.Evaluate(drawnCards);

            // 3. 정렬된 카드 5장을 순서대로 UI PictureBox에 출력
            PictureBox[] cardBoxes = [Card1, Card2, Card3, Card4, Card5];

            for (int i = 0; i < 5; i++) {
                Card card = result.SortedCards[i];
                string imageName = card.GetImageName();

                cardBoxes[i].Image = Properties.Resources.ResourceManager.GetObject(imageName) as Image
                                     ?? Properties.Resources.red_joker;
            }

            MessageBox.Show($"이번 손패는 [{result.HandName}] 입니다!", "족보 판별 결과");
        }
    }
}
