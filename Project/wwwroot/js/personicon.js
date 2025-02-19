document.addEventListener('DOMContentLoaded', function () {
    // 
    // 1. 獲取必要的 DOM 元素
    //    (English: Get required DOM elements)
    //
    const personIcon = document.getElementById('personicon');
    const authSection = document.getElementById('authSection');
    const memberDropdown = document.getElementById('memberDropdown');
    const authTabs = document.querySelectorAll('.auth-tab');
    const authForms = document.querySelectorAll('.auth-form');
    const logoutBtn = document.getElementById('logoutButton');

    //
    // 2. 檢查登入狀態的函數 - 使用後端 API
    //    (English: A function to check login status via backend API)
    //
    async function checkLoginStatus() {
        try {
            const response = await fetch('/FrontMember/CheckLoginStatus');
            const data = await response.json();
            return data.isLoggedIn;
        } catch (error) {
            console.error('檢查登入狀態失敗:', error);
            return false;
        }
    }

    //
    // 3. 點擊會員圖標處理
    //    (English: Handle the click event on the person icon)
    //
    personIcon.addEventListener('click', async function (e) {
        // 阻止 <a> 預設行為 (English: Prevent <a>'s default behavior)
        e.preventDefault();

        const isLoggedIn = await checkLoginStatus();

        if (isLoggedIn) {
            // 
            // 已登入，顯示/隱藏會員選單
            // (English: If logged in, toggle the dropdown menu)
            //
            memberDropdown.style.display =
                memberDropdown.style.display === 'none' ? 'block' : 'none';

            // 同時確保 authSection(登入/註冊表單) 隱藏
            // (English: Also ensure the authSection is hidden)
            if (authSection) authSection.style.display = 'none';

        } else {
            // 
            // 未登入 -> 直接導向到 ~/FrontMember/fcreate (預設行為)
            // (English: If not logged in, let the link go to ~/FrontMember/fcreate)
            //
            // 這裡如果您要顯示前端彈窗，而非跳轉，也可以改寫 
            // (English: If you want a popup instead of a redirect, handle it differently)
            //
            window.location.href = personIcon.getAttribute('href');
        }
    });

    //
    // 4. 註冊/登入表單切換
    //    (English: Toggle between registration and login forms)
    //
    if (authTabs) {
        authTabs.forEach(tab => {
            tab.addEventListener('click', function () {
                // 移除所有標籤的 active 狀態
                // (English: Remove the 'active' class from all tabs)
                authTabs.forEach(t => t.classList.remove('active'));

                // 為當前標籤添加 active 狀態
                // (English: Add 'active' class to the clicked tab)
                this.classList.add('active');

                // 切換對應的表單顯示
                // (English: Show the corresponding form based on data-form)
                const targetForm = this.getAttribute('data-form');
                authForms.forEach(form => {
                    if (form.id === `${targetForm}Form`) {
                        form.style.display = 'block';
                    } else {
                        form.style.display = 'none';
                    }
                });
            });
        });
    }

    //
    // 5. 處理註冊表單提交
    //    (English: Handle the registration form submission)
    //
    const registerForm = document.getElementById('registerForm');
    if (registerForm) {
        registerForm.addEventListener('submit', async function (e) {
            e.preventDefault();
            const formData = new FormData(this);
            try {
                const response = await fetch('/FrontMember/fcreate', {
                    method: 'POST',
                    body: formData
                });
                const result = await response.json();
                if (result.success) {
                    // 註冊成功
                    // (English: Registration success)
                    authSection.style.display = 'none';
                    alert('註冊成功！');
                } else {
                    alert(result.message || '註冊失敗，請稍後再試');
                }
            } catch (error) {
                console.error('註冊失敗:', error);
            }
        });
    }

    //
    // 6. 處理登入表單提交
    //    (English: Handle the login form submission)
    //
    const loginForm = document.getElementById('loginForm');
    if (loginForm) {
        loginForm.addEventListener('submit', async function (e) {
            e.preventDefault();
            const formData = new FormData(this);
            try {
                const response = await fetch('/FrontMember/login', {
                    method: 'POST',
                    body: formData
                });
                const result = await response.json();
                if (result.success) {
                    // 登入成功 -> 隱藏表單並刷新頁面
                    // (English: On success, hide the form and refresh the page)
                    authSection.style.display = 'none';
                    location.reload();
                } else {
                    alert(result.message || '登入失敗，請檢查帳號密碼');
                }
            } catch (error) {
                console.error('登入失敗:', error);
            }
        });
    }

    //
    // 7. 處理登出
    //    (English: Handle the logout action)
    //
    if (logoutBtn) {
        logoutBtn.addEventListener('click', async function (e) {
            e.preventDefault();
            try {
                const response = await fetch('/FrontMember/logout', {
                    method: 'POST'
                });
                const result = await response.json();
                if (result.success) {
                    location.reload();
                }
            } catch (error) {
                console.error('登出失敗:', error);
            }
        });
    }

    //
    // 8. 點擊外部關閉下拉選單和登入表單
    //    (English: Close dropdown and auth forms when clicking outside)
    //
    document.addEventListener('click', function (e) {
        // 如果點擊的不是表單本身且也不是會員 icon
        // (English: If click is not on authSection nor on the person icon)
        if (!authSection?.contains(e.target) &&
            !personIcon.contains(e.target)) {
            if (authSection) authSection.style.display = 'none';
        }
        // 如果點擊的不是下拉選單且也不是會員 icon
        // (English: If click is not on memberDropdown nor on the person icon)
        if (!memberDropdown.contains(e.target) &&
            !personIcon.contains(e.target)) {
            memberDropdown.style.display = 'none';
        }
    });
});
