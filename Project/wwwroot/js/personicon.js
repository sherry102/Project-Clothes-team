// 等待 DOM 載入完成後執行所有程式
document.addEventListener('DOMContentLoaded', function () {
    // 初始化必要的 DOM 元素
    const loginModal = new bootstrap.Modal(document.getElementById('loginModal')); // 登入模態框
    const memberButton = document.getElementById('memberButton');                  // 會員按鈕
    const memberDropdown = document.getElementById('memberDropdown');             // 會員下拉選單
    const loginForm = document.getElementById('loginForm');                       // 登入表單
    const errorMessage = document.getElementById('errorMessage');                 // 錯誤訊息顯示區
    const togglePasswordBtn = document.getElementById('togglePassword');          // 密碼顯示切換按鈕
    const passwordInput = document.getElementById('fpassword');                   // 密碼輸入框
    const logoutButton = document.getElementById('logoutButton');                 // 登出按鈕

    // 設置密碼輸入框的初始狀態
    passwordInput.type = 'password';                                             // 確保密碼輸入為隱藏狀態
    togglePasswordBtn.querySelector('i').className = 'bi bi-eye-slash';          // 設置眼睛圖示為關閉狀態

    // 檢查登入狀態並設置對應的行為
    async function checkLoginStatus() {
        try {
            const response = await fetch('/Account/CheckLoginStatus');
            const data = await response.json();

            if (data.isLoggedIn) {
                // 已登入狀態：移除模態框觸發器，啟用下拉選單功能
                memberButton.removeAttribute('data-bs-toggle');
                memberButton.removeAttribute('data-bs-target');
                memberButton.onclick = function (event) {
                    event.preventDefault();
                    event.stopPropagation();
                    memberDropdown.style.display =
                        memberDropdown.style.display === 'none' ? 'block' : 'none';
                };
            } else {
                // 未登入狀態：設置模態框觸發器
                memberButton.setAttribute('data-bs-toggle', 'modal');
                memberButton.setAttribute('data-bs-target', '#loginModal');
                memberButton.onclick = null;
                memberDropdown.style.display = 'none';
            }
            return data.isLoggedIn;
        } catch (error) {
            console.error('檢查登入狀態時發生錯誤:', error);
            return false;
        }
    }

    // 處理登入表單提交
    loginForm.addEventListener('submit', async function (event) {
        event.preventDefault();
        errorMessage.style.display = 'none';  // 清除之前的錯誤訊息

        // 獲取表單數據
        const formData = new FormData(loginForm);

        try {
            const response = await fetch('/Account/Login', {
                method: 'POST',
                body: formData
            });

            if (response.ok) {
                // 登入成功
                loginModal.hide();                // 隱藏登入視窗
                await checkLoginStatus();         // 更新登入狀態
                window.location.reload();         // 重新載入頁面
            } else {
                // 登入失敗
                const text = await response.text();
                errorMessage.textContent = '帳號或密碼錯誤';
                errorMessage.style.display = 'block';
            }
        } catch (error) {
            console.error('登入過程發生錯誤:', error);
            errorMessage.textContent = '登入過程發生錯誤，請稍後再試';
            errorMessage.style.display = 'block';
        }
    });

    // 密碼顯示切換功能
    togglePasswordBtn.addEventListener('click', function () {
        const type = passwordInput.type === 'password' ? 'text' : 'password';
        passwordInput.type = type;
        // 更新眼睛圖示
        const icon = togglePasswordBtn.querySelector('i');
        icon.className = `bi bi-eye${type === 'password' ? '-slash' : ''}`;
    });

    // 點擊頁面其他區域時關閉下拉選單
    document.addEventListener('click', function (event) {
        if (!memberButton.contains(event.target) &&
            !memberDropdown.contains(event.target)) {
            memberDropdown.style.display = 'none';
        }
    });

    // 登出功能
    logoutButton.addEventListener('click', async function (event) {
        event.preventDefault();
        try {
            const response = await fetch('/Account/Logout', {
                method: 'POST'
            });
            if (response.ok) {
                window.location.reload();  // 登出後重新載入頁面
            }
        } catch (error) {
            console.error('登出時發生錯誤:', error);
        }
    });

    // 頁面載入時檢查登入狀態
    checkLoginStatus();
});