const { createApp } = Vue;

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
            validationRules: { // 新增：表單驗證規則
                faccount: {
                    required: true,
                    message: '請輸入帳號'
                },
                fpassword: {
                    required: true,
                    minLength: 6,
                    maxLength: 10,
                    message: '密碼長度需介於 6-10 字元之間'
                }
            }
        };
    },// 頁面載入時檢查登入狀態
    mounted() {
        const loginModal = document.getElementById('loginModal');
        if (loginModal) {
            this.modalInstance = new bootstrap.Modal(loginModal);

            // 修改監聽事件，確保在 Modal 完全顯示後設置焦點
            loginModal.addEventListener('shown.bs.modal', () => {
                // 使用 setTimeout 確保 DOM 完全渲染
                setTimeout(() => {
                    const accountInput = this.$refs.accountInput;
                    if (accountInput) {
                        accountInput.focus();
                        accountInput.select(); // 自動選中輸入框內容
                        console.log('帳號輸入框焦點設置成功');
                    }
                }, 100);
            });
        }        
    },
    methods: {
        Login: function () {
            let request = {
                faccount: this.faccount,
                fpassword: this.fpassword
            }
            axios.post("https://localhost:7279/Account/Login", request).then(response => {
                console.log(1230)
            }).catch(err => {
                alert(err.message);
            })
        },
        // 改進密碼可見性切換功能
        togglePasswordVisibility() {
            this.passwordVisible = !this.passwordVisible;
        },
        // 改進表單驗證功能
        validateForm() {
            this.errorMessage = '';

            // 帳號驗證
            if (!this.faccount.trim()) {
                this.errorMessage = this.validationRules.faccount.message;
                this.$refs.accountInput?.focus();
                return false;
            }

            // 密碼驗證
            if (!this.fpassword) {
                this.errorMessage = this.validationRules.fpassword.message;
                this.$refs.passwordInput?.focus();
                return false;
            }

            const passwordLength = this.fpassword.length;
            if (passwordLength < 6 || passwordLength > 10) {
                this.errorMessage = this.validationRules.fpassword.message;
                this.$refs.passwordInput?.focus();
                return false;
            }

            return true;
        },      
        async doLogin() {// 改進登入功能
            if (!this.validateForm()) {
                console.log(10);
                return;
            }

            try {
                this.isLoading = true;             
                this.isLoggedIn = true;
                this.modalInstance.hide();
                console.log(20);
            } catch (error) {
                console.error('登入失敗:', error);
                this.errorMessage = '登入失敗，請檢查帳號密碼是否正確';
            } finally {
                this.isLoading = false;
            }
        },
        handleKeyNavigation(currentField, action, event) {
            if (event) {
                event.preventDefault();
            }

            const navigationMap = {
                account: {
                    Enter: () => this.$refs.passwordInput?.focus(),
                    ArrowDown: () => this.$refs.passwordInput?.focus(),
                    ArrowUp: () => this.$refs.loginButton?.focus()
                },
                password: {
                    Enter: () => this.$refs.loginButton?.focus(),
                    ArrowDown: () => this.$refs.loginButton?.focus(),
                    ArrowUp: () => this.$refs.accountInput?.focus()
                },
                loginButton: {
                    Enter: () => this.doLogin(),
                    ArrowDown: () => this.$refs.accountInput?.focus(),
                    ArrowUp: () => this.$refs.passwordInput?.focus()
                }
            };

            const actionFn = navigationMap[currentField]?.[action];
            if (actionFn) {
                actionFn();
            }
        },      
        async doLogin() {
            if (!this.validateForm()) {
                return;
            }

            try {
                this.isLoading = true;
                // 這裡實現實際的登入邏輯
                await this.performLogin();
            }
            catch (error) {
                this.errorMessage = '登入失敗，請檢查帳號密碼';
                console.error('登入錯誤:', error);
            }
            finally {
                this.isLoading = false;
            }
        }
    },
}).mount('#vueLogin');
