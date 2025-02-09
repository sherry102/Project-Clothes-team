const chatApp = Vue.createApp({
    data() {
        return {
            isVisible: false,
            isBubbleVisible: false,
            isTyping: false,
            message: "",
            messages: [
                {
                    type: "received",
                    content: "您好！我是智能助理Unicorn，很高興為您服務。",
                },
                {
                    type: "received",
                    content: "請問有什麼我可以協助您的嗎？",
                },
            ],
            defaultReplies: {
                你好: "您好！很高興為您服務。",
                hi: "Hi！有什麼我可以幫您的嗎？",
                hello: "Hello！請問需要什麼協助呢？",
                謝謝: "不客氣！還有什麼我可以幫您的嗎？",
                掰掰: "再見！祝您有愉快的一天！",
                再見: "再見！如果還有問題歡迎隨時詢問。",
            },
        };
    },
    mounted() {
        // 5秒後顯示氣泡
        setTimeout(() => {
            if (!this.isVisible) {
                // 只在聊天視窗未開啟時顯示氣泡
                this.isBubbleVisible = true;
            }
        }, 5000);

        // 監聽點擊外部
        document.addEventListener("click", this.handleOutsideClick);
    },
    beforeUnmount() {
        document.removeEventListener("click", this.handleOutsideClick);
    },
    methods: {
        toggleChat() {
            this.isVisible = true;
            this.isBubbleVisible = false;
            this.$nextTick(() => {
                this.$refs.chatInput.focus();
            });
        },
        closeChat() {
            this.isVisible = false;
            // 關閉時重置所有狀態
            this.message = "";
            this.isTyping = false;
        },
        handleOutsideClick(e) {
            const chatWidget = this.$refs.chatWidget;
            const chatToggle = this.$refs.chatToggle;
            if (
                this.isVisible &&
                chatWidget &&
                !chatWidget.contains(e.target) &&
                !chatToggle.contains(e.target)
            ) {
                this.closeChat();
            }
        },
        getSmartReply(message) {
            const lowerMsg = message.toLowerCase();

            if (lowerMsg.includes("價格") || lowerMsg.includes("費用")) {
                return "關於價格方案，建議您可以參考我們的方案頁面，或是告訴我您的需求，我可以為您推薦最適合的方案。";
            }
            if (lowerMsg.includes("如何") || lowerMsg.includes("怎麼")) {
                return "需要更詳細了解您的問題，可以告訴我您想知道的具體內容嗎？";
            }
            if (lowerMsg.includes("聯絡") || lowerMsg.includes("客服")) {
                return "您可以撥打我們的服務專線09999999999999，或留下您的聯絡方式，我們會盡快與您聯繫。";
            }

            for (let key in this.defaultReplies) {
                if (lowerMsg.includes(key)) {
                    return this.defaultReplies[key];
                }
            }

            return "抱歉，我可能沒有完全理解您的問題。您可以換個方式描述，或直接聯繫我們的客服人員。";
        },
        async SendMessage() {
            if (!this.message.trim()) return;

            // 添加用戶訊息
            this.messages.push({
                type: "sent",
                content: this.message,
            });

            const userMessage = this.message;
            this.message = "";
            this.isTyping = true;

            // 滾動到底部
            await this.$nextTick();
            this.scrollToBottom();

            // 延遲回覆
            setTimeout(() => {
                this.isTyping = false;
                this.messages.push({
                    type: "received",
                    content: this.getSmartReply(userMessage),
                });

                this.$nextTick(() => {
                    this.scrollToBottom();
                });
            }, 1000);
        },
        scrollToBottom() {
            const container = this.$refs.messagesContainer;
            container.scrollTop = container.scrollHeight;
        },
    },
}).mount("#chatApp");
