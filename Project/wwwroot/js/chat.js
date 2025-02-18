
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
            showChatButton: false,
            isRealCustomerVisible: false
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

        // 修改點擊事件監聽器
        document.addEventListener("click", (e) => {
            if (this.$refs.chatWidget && this.$refs.chatToggle) {
                if (this.isVisible &&
                    !this.$refs.chatWidget.contains(e.target) &&
                    !this.$refs.chatToggle.contains(e.target) &&
                    !e.target.closest('.chat-bubble')) {
                    this.colseChat();
                }
            }
        });
        // 監聽消息變化
        //this.$watch('messages', () => {
        //    this.$nextTick(() => {
        //        this.scrollToBottom();
        //    });
        //}, { deep: true });
        this.$watch(
            () => this.messages.length, // 監聽 messages 陣列長度變化
            () => {
                this.$nextTick(() => this.scrollToBottom());
            }
        );
    },
    beforeUnmount() {
        document.removeEventListener("click", this.handleOutsideClick);
    },
    methods: {
        toggleChat() {
            this.isVisible = !this.isVisible;
            if (this.isVisible) {
                this.isBubbleVisible = false;
                this.isRealCustomerVisible = false;
                this.$nextTick(() => this.scrollToBottom());
            }
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
            if (this.isVisible && chatWidget && !chatWidget.contains(e.target) && !chatToggle.contains(e.target)) {
                this.closeChat();
            }
        },
        getSmartReply(message) {
            const lowerMsg = message.toLowerCase();
            if (lowerMsg.includes("會員")) {
                this.showChatButton = true;
                return "您是否需要會員相關協助？請點擊下方按鈕與真人客服聯繫。";
            }
            if (lowerMsg.includes("訂單")) {
                this.showChatButton = true;
                return "關於您的訂單，請點擊下方按鈕與客服人員聯繫。";
            }
            if (lowerMsg.includes("活動")) {
                this.showChatButton = true;
                return "關於我們的活動，請點擊下方按鈕與客服人員聯繫。";
            }
            if (lowerMsg.includes("客製")) {
                this.showChatButton = true;
                return "我們提供客製化服務，請點擊下方按鈕與客服人員聯繫。";
            }
            if (lowerMsg.includes("價格") || lowerMsg.includes("費用")) {
                return "關於價格方案，建議您可以參考我們的方案頁面，或是告訴我您的需求，我可以為您推薦最適合的方案。";
            }
            if (lowerMsg.includes("如何") || lowerMsg.includes("怎麼")) {
                return "需要更詳細了解您的問題，可以告訴我您想知道的具體內容嗎？";
            }
            if (lowerMsg.includes("聯絡") || lowerMsg.includes("客服")) {
                return "請點擊下方按鈕連接真人客服";
            }
            for (let key in this.defaultReplies) {
                if (lowerMsg.includes(key)) {
                    return this.defaultReplies[key];
                }
            }
            return "抱歉，我可能沒有完全理解您的問題。您可以換個方式描述，或直接聯繫我們的客服人員。";
        },
        async sendMessage() {
            if (!this.message.trim()) return;
            // console.log(this.messages);
            // 檢查是否包含"客服"關鍵字
            const isCustomerService = this.message.includes("客服");
            // 添加用戶訊息
            this.messages.push({ type: "sent", content: this.message });
            console.log("發送訊息:", this.messages);
            const userMessage = this.message;
            this.message = "";
            this.isTyping = true;
            // 立即滾動到底部
            await this.$nextTick();
            this.scrollToBottom();
            //延後回覆
            setTimeout(() => {
                this.isTyping = false;
                if (isCustomerService) {
                    // 如果包含"客服"關鍵字，顯示帶按鈕的回覆
                    this.messages.push({
                        type: "received",
                        content: "請點擊下方按鈕連接真人客服",
                        showButton: true
                    });
                }
                else {
                    // 否則顯示智能回覆                       
                    const reply = this.getSmartReply(userMessage);
                    this.messages.push({
                        type: "received",
                        content: reply,
                        showButton: this.showChatButton
                    });
                }
                this.$nextTick(() => this.scrollToBottom());
            }, 1000);
        },
        // 新增連接真人客服方法
        connectToRealService() {
            // 這裡可以添加連接真人客服的邏輯
            // alert("正在為您連接真人客服...");
            let timerInterval;
            Swal.fire({
                title: "系統通知",
                html: "正在為您連接真人客服 <b></b> milliseconds.",
                timer: 2000, // 2 秒倒數
                timerProgressBar: true,
                didOpen: () => {
                    Swal.showLoading();
                    const timer = Swal.getPopup().querySelector("b");
                    timerInterval = setInterval(() => {
                        timer.textContent = `${Swal.getTimerLeft()}`;
                    }, 100);
                },
                willClose: () => {
                    clearInterval(timerInterval);
                },
            })
            .then((result) => {
                if (result.dismiss === Swal.DismissReason.timer) {
                    // 這裡可以加入真正的客服連接邏輯
                    console.log("真人客服已連接！");
                    this.addSystemMessage("已進入真人客服！！"); // 新增一條系統訊息
                    // 例如開啟客服視窗的函數：
                    // openCustomerServiceChat();                        
                }
            });
        },
        addSystemMessage(message) {
            const chatBox = document.querySelector(".chat-widget-context"); // 替換成你的聊天視窗選擇器
            const systemMsg = document.createElement("div");
            systemMsg.className = "system-message"; // 可加 CSS 調整樣式
            systemMsg.innerText = message;
            chatBox.appendChild(systemMsg);
        },
        scrollToBottom() {
            this.$nextTick(() => {
                const container = this.$refs.messagesContainer;
                if (container) {
                    setTimeout(() => {
                        container.scrollTop = container.scrollHeight;
                    }, 200); // 給予一個小延遲確保 DOM 已更新
                }
            });
        },
        startRealCustomerService() {
            this.isRealCustomerVisible = true;
            this.isVisible = false;
        }
    }
}).mount("#chatApp");