const {createApp} = Vue;
   createApp({
        data() {
        return {
            faccount: '',
            fpassword: '',
            passwordVisible: false,// 密碼是否顯示明文
            isLoading: false,// 是否顯示 Loading 狀態
            errorMessage: '', // 顯示錯誤訊息
            isLoggedIn: false,// 追踪用戶是否已登入
            memberInfo: null,  // 存儲會員信息           
            isiconBubbleVisible: true,// 控制人像圖標是否顯示 
            showLoginModal: false   // 新增：控制登入 Modal 的顯示狀態
            };
        },// 頁面載入時檢查登入狀態
        mounted() {
            this.checkLoginStatus();
            // 監聽 Modal 顯示事件
            const loginModal = document.getElementById('loginModal'); // 獲取登入模態框元素
            if (loginModal) {// 為模態框顯示事件添加監聽器
                loginModal.addEventListener('shown.bs.modal', () => {
                    this.$nextTick(() => {// 在下一個 DOM 更新循環中執行
                    const accountInput = this.$refs.accountInput;
                        if (accountInput) {
                            accountInput.focus();
                        }
                    });
                });               
            }
       },// 定義組件的方法
       methods: {
           handleMemberClick(event) {
               if (!this.isLoggedIn) {
                   this.showLoginModal = true; // 更新：使用 Vue 的數據屬性控制 Modal
                   this.$nextTick(() => {
                       const loginModal = new bootstrap.Modal(document.getElementById('loginModal'));
                       loginModal.show();
                   });
               }
           },
           // 重置表單數據
           resetForm() {
               this.faccount = '';
               this.fpassword = '';
               this.errorMessage = '';
               this.showLoginModal = false;
           },
           async doLogin() {
               if (!this.faccount || !this.fpassword) {
                   alert('請填寫完整的帳號和密碼！');
                   return;
               }
               this.isLoading = true;// 設置加載狀態  
               try {
                   const response = await fetch('/Account/Login', {
                       method: 'POST',
                       headers: {
                           'Content-Type': 'application/json',
                           'X-Requested-With': 'XMLHttpRequest'
                       },
                       credentials: 'include',
                       body: JSON.stringify({
                           faccount: this.faccount.trim(),
                           fpassword: this.fpassword
                       })
                   });
                   const data = await response.json();
                   if (data.success) {
                       this.isLoggedIn = true;
                       await this.checkLoginStatus();
                       this.showLoginModal = false; // 更新：關閉 Modal
                       window.location.href = data.redirectUrl || '/FrontHome/FrontIndex';
                   } else {
                       this.errorMessage = data.message || '登入失敗，請檢查帳號密碼！';
                       alert(this.errorMessage);
                   }
               } catch (error) {
                   console.error('Login error:', error);
                   this.errorMessage = '系統發生錯誤，請稍後再試！';
                   alert(this.errorMessage);
               } finally {
                   this.isLoading = false;
               }
           },// 切換密碼可見性                   
           togglePasswordVisibility() {
               this.passwordVisible = !this.passwordVisible;
           },
           handleKeyNavigation(currentField, action, event) {// 處理鍵盤導航
               if (event) {// 阻止默認事件
                   event.preventDefault();
               }
               const navigationMap = {
                   account: {
                       Enter: 'password',
                       ArrowDown: 'password',
                       ArrowUp: 'loginButton'
                   },
                   password: {
                       Enter: 'loginButton',
                       ArrowDown: 'loginButton',
                       ArrowUp: 'account'
                   },
                   loginButton: {
                       Enter: 'doLogin',
                       ArrowDown: 'account',
                       ArrowUp: 'password'
                   }
               };
               const targetField = navigationMap[currentField][action];// 找到目標欄位

               if (targetField === 'doLogin') { // 如果目標是 "doLogin" 代表要直接執行登入
                   this.doLogin();
                   return;
               } // 在下一個 DOM 更新循環中切換焦點
               this.$nextTick(() => {
                   const element = targetField === 'loginButton'
                       ? this.$refs.loginButton
                       : this.$refs[`${targetField}Input`];
                   if (element) { // 如果有抓到對應的元素，則讓該元素獲得焦點
                       element.focus();
                   }
               });
           }
       }
   }).mount('#vueLogin');
