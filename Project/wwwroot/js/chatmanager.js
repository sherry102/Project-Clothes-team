
    // 定義檔案類型對應表，用於上傳檔案時的類型選擇和驗證
    const fileTypeMap = {
        'file': {label: '檔案', accept: '*/*' },
    'image': {label: '圖片', accept: 'image/*' },
    'video': {label: '影片', accept: 'video/*' }
        };

    // 建立初始聊天資料模擬，整合原本第一個JS的結構和資料
    const initialChatData = {
        proposal: {
        1: {
        name: "陳蟑言",
    avatar: "https://www.shutterstock.com/image-photo/adorable-piglet-peeking-over-wooden-260nw-2526412985.jpg",
    projectName: "客製化服飾問題",
    status: "進行中",
    messages: [
    {
        type: "received",
    content: "請問客製化服飾什麼時候出貨？",
    time: "下午 2:02",
    timestamp: new Date().toLocaleTimeString('en-US', {hour12: false })
                        },
    ],
                },
    2: {
        name: "謝欣達",
    avatar: "https://yt3.googleusercontent.com/o0D3C98I6Z6I0SG8HJIZoM7bOGNVkl8MEtFGrh65U2X07x5Eg8EtEpV5GH7X5SWuilb9UBHF=s900-c-k-c0x00ffffff-no-rj",
    projectName: "會員註銷",
    status: "已完成",
    messages: [
    {
        type: "received",
    content: "我的會員要註銷",
    time: "上午 9:20",
    timestamp: new Date().toLocaleTimeString('en-US', {hour12: false })
                        },
    ],
                },
            },
        };

    // 創建 Vue 應用
    const chatApp = Vue.createApp({
        data() {
                return {
        userId: "2", // 目前登入的會員ID
    userName: "陳蟑言", // 目前登入的會員名稱
    chatId: null, // 目前的聊天室ID
    chatType: "proposal", // 目前的聊天類型，預設為提案訊息
    chatData: JSON.parse(JSON.stringify(initialChatData)), // 所有聊天資料，使用深拷貝避免修改原始資料
    message: "", // 輸入框中的訊息內容
    messages: [], // 目前顯示的訊息列表
    isTyping: false, // 用來顯示用戶是否正在輸入訊息
    hubConnection: null, // SignalR 連接對象
    lastEnterTime: null, // 追蹤最後一次按Enter的時間，用於雙擊Enter發送訊息功能
    members: [] // 儲存會員列表
                }
            },
    // 計算屬性
    computed: {
        // 獲取當前聊天室的完整資訊
        currentChat() {
                    if (!this.chatId || !this.chatData[this.chatType]) return null;
    return this.chatData[this.chatType][this.chatId] || null;
                },
    // 判斷是否有選中聊天室
    hasChatSelected() {
                    return !!this.currentChat;
                },
    // 根據當前時間生成問候語
    greeting() {
                    const currentHour = new Date().getHours();
                    if (currentHour >= 5 && currentHour < 12) {
                        return "早安";
                    } else if (currentHour >= 12 && currentHour < 18) {
                        return "午安";
                    } else {
                        return "晚安";
                    }
                },
            },
    // 監聽器
    watch: {
        // 監聽當前聊天室變化，更新顯示的訊息
        currentChat: {
        handler(newChat) {
                        if (newChat) {
        this.messages = newChat.messages || [];
                            // 滾動到最新訊息
                            this.$nextTick(() => this.scrollToBottom());
                        } else {
        this.messages = [];
                        }
                    },
    immediate: true, // 初始化時立即執行
    deep: true // 深度監聽，確保監聽到陣列和物件內部的變化
                },
            },
    // 組件載入時初始化
    async mounted() {
                try {
        // 連接 SignalR
        await this.initializeSignalRConnection();

    // 初始化UI和事件綁定
    this.initializeUI();

    // 自動選擇第一個聊天
    this.selectFirstChat();

    // 監聽消息變化以自動滾動
    this.$watch(
                        () => this.messages.length,
                        () => this.$nextTick(this.scrollToBottom)
    );
                }
    catch (error) {
        console.error("初始化失敗:", error);
    Swal.fire({
        title: '錯誤',
    text: '連接聊天服務失敗，請重新整理頁面',
    icon: 'error'
                    });
                }
            },
    methods: {
        // 初始化UI和事件綁定
        initializeUI() {
                    // 綁定切換聊天類型標籤的事件
                    const typeTabs = document.querySelectorAll(".type-tab");
                    if (typeTabs && typeTabs.length > 0) {
        typeTabs.forEach(tab => {
            tab.addEventListener("click", (e) => {
                const type = e.target.dataset.type;
                this.switchChatType(type);
            });
        });
                    }

    // 綁定輸入框的特殊處理事件
    const chatInput = document.querySelector(".chat-input");
    if (chatInput) {
        chatInput.addEventListener("keydown", this.handleKeyPress);
    chatInput.addEventListener("input", this.adjustInputHeight);
                    }

    // 初始化聊天列表
    this.initializeChatLists();
                },

    // 初始化聊天列表，將聊天資料渲染到UI上
    initializeChatLists() {
                    // 清空現有的聊天列表
                    const chatLists = document.querySelectorAll(".chat-list");
                    chatLists.forEach(list => {
                        const type = list.dataset.type;
    // 清空列表
    list.innerHTML = "";

    // 遍歷聊天資料並創建列表項
    if (this.chatData[type]) {
        Object.entries(this.chatData[type]).forEach(([id, chat]) => {
            const chatItem = this.createChatListItem(id, type, chat);
            list.appendChild(chatItem);
        });
                        }
                    });
                },

    // 創建聊天列表項
    createChatListItem(id, type, chat) {
                    const lastMessage = chat.messages[chat.messages.length - 1] || {time: '', content: '' };

    const chatItem = document.createElement("div");
    chatItem.className = "chat-item";
    chatItem.dataset.chatId = id;
    chatItem.dataset.type = type;

    chatItem.innerHTML = `
    <div class="chat-item-left">
        <div class="chat-avatar">
            <img src="${chat.avatar}" alt="${chat.name}">
        </div>
    </div>
    <div class="chat-item-right">
        <div class="chat-item-header">
            <span class="chat-item-name">${chat.name}</span>
            <span class="chat-item-time">${lastMessage.time}</span>
        </div>
        <div class="chat-item-message">${lastMessage.content}</div>
    </div>
    `;

                    // 添加點擊事件
                    chatItem.addEventListener("click", () => this.selectChat(id, type));

    return chatItem;
                },

    // 選擇聊天室
    selectChat(chatId, type) {
        // 更新當前的聊天ID和類型
        this.chatId = chatId;
    this.chatType = type;

                    // 更新UI選中狀態
                    document.querySelectorAll(".chat-item").forEach(item => {
        item.classList.remove("active");
                    });

    const selectedItem = document.querySelector(`.chat-item[data-chat-id="${chatId}"][data-type="${type}"]`);
    if (selectedItem) {
        selectedItem.classList.add("active");
                    }

    // 更新聊天室頭部信息
    this.updateChatHeader();

    // 更新SignalR連接的聊天室
    this.joinChatRoom(chatId);
                },

    // 自動選擇第一個聊天
    selectFirstChat() {
                    const firstChat = document.querySelector(`.chat-list[data-type="${this.chatType}"] .chat-item`);
    if (firstChat) {
                        const chatId = firstChat.dataset.chatId;
    const type = firstChat.dataset.type;
    this.selectChat(chatId, type);
                    }
                },

    // 切換聊天類型
    switchChatType(type) {
                    if (!type || this.chatType === type) return;

    // 更新當前類型
    this.chatType = type;

                    // 更新UI
                    document.querySelectorAll(".type-tab").forEach(tab => {
        tab.classList.toggle("active", tab.dataset.type === type);
                    });

                    document.querySelectorAll(".chat-list").forEach(list => {
        list.classList.toggle("active", list.dataset.type === type);
                    });

    // 自動選擇第一個聊天
    this.selectFirstChat();
                },

    // 更新聊天室頭部信息
    updateChatHeader() {
                    if (!this.currentChat) return;

    // 更新用戶頭像和名稱
    const avatarElement = document.querySelector(".chat-user-avatar");
    const nameElement = document.querySelector(".chat-user-name");
    if (avatarElement) avatarElement.src = this.currentChat.avatar;
    if (nameElement) nameElement.textContent = this.currentChat.name;

    // 更新專案資訊
    const projectNameElement = document.querySelector(".project-name");
    const statusElement = document.querySelector(".project-status");

    if (projectNameElement) projectNameElement.textContent = this.currentChat.projectName;
    if (statusElement) {
        statusElement.textContent = this.currentChat.status;
    statusElement.className = `project-status ${this.currentChat.status}`;
                    }
                },

    // 初始化 SignalR 連線的方法
    async initializeSignalRConnection() {
                    try {
        // 建立 SignalR 連線
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(`https://localhost:7279/ChatRoom?room=${this.userId}`) // 設定 SignalR Hub 連線
            .withAutomaticReconnect([0, 2000, 5000, 10000, null]) // 自動重連設置
            .configureLogging(signalR.LogLevel.Debug) // 開啟詳細的日誌記錄
            .build();

    // 註冊接收訊息的事件處理器
    this.hubConnection.on("UpdContent", this.receiveMessage);

    // 啟動連接
    await this.hubConnection.start();
    console.log("SignalR連接成功");
                    }
    catch (err) {
        // 錯誤處理
        console.error("連線錯誤:", err);
    // 將錯誤訊息加入聊天室
    this.addSystemMessage("無法連接到客服系統，請稍後再試");

    // 顯示錯誤提示
    Swal.fire({
        title: '錯誤',
    text: '無法連接到客服系統，請稍後再試',
    icon: 'error'
                        });

                        // 3秒後重試
                        setTimeout(() => {
        this.initializeSignalRConnection();
                        }, 3000);
                    }
                },

    // 加入特定聊天室
    async joinChatRoom(roomId) {
                    if (!this.hubConnection || !roomId) return;

    try {
        // 呼叫 SignalR 方法來加入特定聊天室
        await this.hubConnection.invoke("JoinRoom", roomId.toString());
    console.log(`加入聊天室 ${roomId} 成功`);
                    } catch (error) {
        console.error(`加入聊天室失敗:`, error);
                    }
                },

    // 接收訊息處理
    receiveMessage(msg) {
        console.log("收到伺服器訊息: ", msg);

    // 檢查消息是否有效
    if (!msg || !msg.message) return;

    // 根據消息類型添加不同樣式
    const messageObj = {
        type: msg.user === this.userId ? "sent" :
    msg.user === "System" ? "system" : "received",
    content: msg.message,
    time: this.formatMessageTime(),
    timestamp: msg.timestamp || new Date().toLocaleTimeString('en-US', {hour12: false })
                    };

    // 如果目前有選擇聊天室，則添加訊息到當前聊天
    if (this.currentChat && this.messages) {
        // 添加到Vue的data中，確保視圖更新
        this.messages.push(messageObj);

    // 同時更新chatData中的訊息
    this.currentChat.messages.push(messageObj);

    // 更新左側列表顯示的最新訊息
    this.updateChatListItem(this.chatId, this.chatType, messageObj.content, messageObj.time);

    // 滾動到底部
    this.$nextTick(this.scrollToBottom);
                    }
                },

    // 新增系統訊息
    addSystemMessage(content) {
                    const systemMsg = {
        type: "system",
    content: content,
    time: this.formatMessageTime(),
    timestamp: new Date().toLocaleTimeString('en-US', {hour12: false })
                    };

    this.messages.push(systemMsg);

    // 如果有選中的聊天室，也添加到聊天室資料中
    if (this.currentChat) {
        this.currentChat.messages.push(systemMsg);
                    }

    // 滾動到底部
    this.$nextTick(this.scrollToBottom);
                },

    // 發送訊息
    async sendMessage() {
                    // 檢查訊息是否為空或是否有選中聊天室
                    if (!this.message.trim() || !this.hasChatSelected) return;

    const messageContent = this.message.trim();

    try {
                        // 格式化當前時間
                        const messageTime = this.formatMessageTime();

    // 創建新訊息物件
    const newMessage = {
        type: "sent",
    content: messageContent,
    time: messageTime,
    timestamp: new Date().toLocaleTimeString('en-US', {hour12: false })
                        };

    // 儲存到本地聊天室資料中
    this.currentChat.messages.push(newMessage);

    // 更新左側列表顯示的最新訊息
    this.updateChatListItem(this.chatId, this.chatType, messageContent, messageTime);

    // 同時呼叫 SignalR 方法發送到服務器
    await this.hubConnection.invoke("SendMessage", this.userId, messageContent);

    // 清空輸入框
    this.message = "";

    // 重置輸入框高度
    this.adjustInputHeight({target: document.querySelector(".chat-input") });

    // 重置雙Enter狀態
    this.lastEnterTime = null;

    // 重置輸入狀態
    this.isTyping = false;

                    }
    catch (error) {
        console.error("發送訊息失敗:", error);
    // 顯示錯誤提示
    Swal.fire({
        title: '錯誤',
    text: '發送訊息失敗，請稍後再試',
    icon: 'error'
                        });
                    }
                },

    // 更新左側列表項的最新訊息
    updateChatListItem(chatId, type, content, time) {
                    const chatItem = document.querySelector(`.chat-item[data-chat-id="${chatId}"][data-type="${type}"]`);
    if (chatItem) {
                        const messageElement = chatItem.querySelector(".chat-item-message");
    const timeElement = chatItem.querySelector(".chat-item-time");
    if (messageElement) messageElement.textContent = content;
    if (timeElement) timeElement.textContent = time;
                    }
                },

    // 格式化訊息時間為 "上午/下午 HH:MM" 格式
    formatMessageTime() {
    const now = new Date();
    const hours = now.getHours();
    const minutes = now.getMinutes();
                    const period = hours >= 12 ? "下午" : "上午";
    const displayHours = hours % 12 || 12;
    return `${period} ${displayHours}:${minutes.toString().padStart(2, "0")}`;
                },

    // 處理鍵盤按鍵事件，特別是Enter鍵發送訊息
    handleKeyPress(e) {
                    if (e.key === "Enter" && !e.shiftKey) {
        e.preventDefault(); // 防止換行

    const currentTime = Date.now();

    if (!this.lastEnterTime) {
        // 第一次按 Enter
        this.lastEnterTime = currentTime;
    // 顯示提示訊息
    if (this.message.trim()) {
        e.target.style.backgroundColor = "#f0f8ff"; // 輕微變色提示
                            }
                        } else {
                            // 第二次按 Enter
                            const timeDiff = currentTime - this.lastEnterTime;

    if (timeDiff < 500) {
                                // 如果兩次按 Enter 間隔太短，忽略
                                return;
                            }

    // 第二次 Enter 一定發送
    this.sendMessage();
    this.lastEnterTime = null; // 重置 lastEnterTime
                        }
                    } else if (e.key !== "Enter") {
        // 按其他鍵時重置 lastEnterTime
        this.lastEnterTime = null;

    // 如果有文字，設定為正在輸入狀態
    if (this.message.trim()) {
        this.isTyping = true;
                        } else {
        this.isTyping = false;
                        }
                    }
                },

    // 動態調整輸入框高度
    adjustInputHeight(e) {
                    const inputElement = e.target;
    // 先重置高度
    inputElement.style.height = "40px";
    // 然後設置新高度，但不超過120px
    const newHeight = Math.min(inputElement.scrollHeight, 120);
    inputElement.style.height = newHeight + "px";
                },

    // 處理檔案上傳
    async handleFileUpload(event, fileType) {
                    const fileInput = event.target;
    if (!fileInput.files || fileInput.files.length === 0) return;
    const file = fileInput.files[0];

    // 檢查檔案類型（如果需要限制）
    if (fileTypeMap[fileType] && !file.type.startsWith(fileType.split('/')[0] + "/")) {
        Swal.fire({
            title: '錯誤',
            text: `請選擇${fileTypeMap[fileType].label}檔案`,
            icon: 'error'
        });
    return;
                    }

    // 顯示上傳中訊息
    this.addSystemMessage(`正在上傳${fileTypeMap[fileType]?.label ?? "檔案"}...`);

    try {
                        const formData = new FormData();
    formData.append('file', file);
    const response = await fetch('/Chat/UploadFile', {method: 'POST', body: formData });

    // 移除載入訊息
    this.messages.pop();
    if (this.currentChat) {
        this.currentChat.messages.pop();
                        }

    if (!response.ok) throw new Error(`HTTP 錯誤！狀態碼: ${response.status}`);

    const result = await response.json();
    if (result.success) {
        let fileMessageContent = "";
    if (fileType === "image") {
        fileMessageContent = `<img src="${result.fileUrl}" alt="${result.originalName}" class="chat-image height:50% width:50%"/>`;
                            }
    else if (fileType === "video") {
        fileMessageContent = `<video controls class="chat-video"><source src="${result.fileUrl}" type="${result.contentType}"></video>`;
                            }
    else {
        fileMessageContent = `<div class="file-link"><a href="${result.fileUrl}" target="_blank" download="${result.originalName}"><i class="fas fa-file"></i> ${result.originalName}</a></div>`;
                            }

    // 發送到聊天室
    await this.hubConnection.invoke("SendMessage", this.userId, fileMessageContent);
                        }
    else {
        this.addSystemMessage(`檔案上傳失敗：${result.message}`);
                        }
                    }
    catch (error) {
        console.error("檔案上傳失敗:", error);
    // 移除載入訊息如果還存在
    const lastMsg = this.messages[this.messages.length - 1];
    if (lastMsg && lastMsg.type === "system" && lastMsg.content.includes("正在上傳")) {
        this.messages.pop();
    if (this.currentChat) {
        this.currentChat.messages.pop();
                            }
                        }

    this.addSystemMessage("檔案上傳失敗，請稍後再試");
                    }
    finally {
        this.scrollToBottom();
    fileInput.value = ''; // 清空輸入框
                    }
                },

    // 觸發檔案上傳輸入框
    triggerFileInput(fileType) {
                    const fileInput = document.createElement('input');
    fileInput.type = 'file';
    fileInput.style.display = 'none';
    if (fileTypeMap[fileType]) {
        fileInput.accept = fileTypeMap[fileType].accept;
                    }

                    fileInput.addEventListener('change', (event) => this.handleFileUpload(event, fileType));
    document.body.appendChild(fileInput);
    fileInput.click();

                    // 100ms後移除這個臨時節點
                    setTimeout(() => document.body.removeChild(fileInput), 100);
                },

    // 滾動到最新消息
    scrollToBottom() {
                    // 優先使用Vue refs引用的元素
                    const container = this.$refs.messagesContainer;
    if (container) {
        container.scrollTop = container.scrollHeight;
    return;
                    }

    // 作為備用，直接查詢DOM
    const chatMessages = document.querySelector('.chat-messages');
    if (chatMessages) {
        chatMessages.scrollTop = chatMessages.scrollHeight;
                    }
                }
            }
        });

    // 啟動 Vue 應用
    chatApp.mount("#chatApp");