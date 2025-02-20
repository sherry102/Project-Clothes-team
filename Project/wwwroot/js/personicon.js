document.addEventListener('DOMContentLoaded', function () { 
    document.getElementById('personicon').addEventListener('click', function (e) { // 當使用者點擊個人圖示時
        e.preventDefault();       
        fetch('/Account/CheckLoginStatus') // 呼叫AccountController的CheckLoginStatus方法
            .then(response => response.json())
            .then(data =>
            {
                if (data.isLoggedIn == true) {                  
                    fetch('/FrontMember/HandleProfileClick') // 已登入，呼叫FrontMember控制器的HandleProfileClick方法獲取選單
                        .then(response => response.json())
                        .then(menuData =>
                        {
                            if (menuData.success) {
                                const dropdown = document.getElementById('memberDropdown');
                                dropdown.innerHTML = '';

                                menuData.data.forEach(item => {
                                    const menuItem = document.createElement('a');
                                    menuItem.href = item.url;
                                    menuItem.className = 'dropdown-item py-2';

                                    if (item.text === "登出") {
                                        menuItem.id = 'logoutButton';
                                        menuItem.addEventListener('click', handleLogout);
                                    }

                                    menuItem.innerHTML = `
                                        <div class="d-flex align-items-center" style="gap: 12px;">
                                            <div style="width: 24px; height: 24px;" class="d-flex align-items-center justify-content-center">
                                                <i class="bi bi-${getIconForMenuItem(item.text)}"></i>
                                            </div>
                                            <span>${item.text}</span>
                                        </div>
                                    `;
                                    dropdown.appendChild(menuItem);
                                });

                                dropdown.style.display = 'block';
                            }
                        })
                        .catch(error => {
                            console.error('Error fetching menu:', error);
                        });
                } else {                 
                    window.location.href = '/FrontMember/fcreate';
                }
            })
            .catch(error => {
                console.error('Error checking login status:', error);                
                window.location.href = '/FrontMember/fcreate';// 發生錯誤時，導向 fcreate
            });
    });  
    function getIconForMenuItem(text) {// 根據選單文字取得對應圖示
        switch (text) {
            case '個人資料': return 'person';
            case '我的訂單': return 'bag';
            case '優惠券': return 'ticket-perforated';
            case '登出': return 'box-arrow-right';
            default: return 'chevron-right';
        }
    } 
    function handleLogout(e) {// 處理登出
        e.preventDefault();
        fetch('/Account/Logout', {
            method: 'POST'
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {                    
                    window.location.href = '/FrontHome/FrontIndex';
                } else {
                    console.error('Logout failed:', data.message);
                }
            })
            .catch(error => {
                console.error('Error during logout:', error);
            });
    }

    // 點擊外部關閉下拉選單
    document.addEventListener('click', function (event) {
        const dropdown = document.getElementById('memberDropdown');
        const personIcon = document.getElementById('personicon');
        if (dropdown.style.display === 'block' &&
            !dropdown.contains(event.target) &&
            !personIcon.contains(event.target)) {
            dropdown.style.display = 'none';
        }
    });
});
