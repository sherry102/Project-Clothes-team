// ���� DOM ���J���������Ҧ��{��
document.addEventListener('DOMContentLoaded', function () {
    // ��l�ƥ��n�� DOM ����
    const loginModal = new bootstrap.Modal(document.getElementById('loginModal')); // �n�J�ҺA��
    const memberButton = document.getElementById('memberButton');                  // �|�����s
    const memberDropdown = document.getElementById('memberDropdown');             // �|���U�Կ��
    const loginForm = document.getElementById('loginForm');                       // �n�J���
    const errorMessage = document.getElementById('errorMessage');                 // ���~�T����ܰ�
    const togglePasswordBtn = document.getElementById('togglePassword');          // �K�X��ܤ������s
    const passwordInput = document.getElementById('fpassword');                   // �K�X��J��
    const logoutButton = document.getElementById('logoutButton');                 // �n�X���s

    // �]�m�K�X��J�ت���l���A
    passwordInput.type = 'password';                                             // �T�O�K�X��J�����ê��A
    togglePasswordBtn.querySelector('i').className = 'bi bi-eye-slash';          // �]�m�����ϥܬ��������A

    // �ˬd�n�J���A�ó]�m�������欰
    async function checkLoginStatus() {
        try {
            const response = await fetch('/Account/CheckLoginStatus');
            const data = await response.json();

            if (data.isLoggedIn) {
                // �w�n�J���A�G�����ҺA��Ĳ�o���A�ҥΤU�Կ��\��
                memberButton.removeAttribute('data-bs-toggle');
                memberButton.removeAttribute('data-bs-target');
                memberButton.onclick = function (event) {
                    event.preventDefault();
                    event.stopPropagation();
                    memberDropdown.style.display =
                        memberDropdown.style.display === 'none' ? 'block' : 'none';
                };
            } else {
                // ���n�J���A�G�]�m�ҺA��Ĳ�o��
                memberButton.setAttribute('data-bs-toggle', 'modal');
                memberButton.setAttribute('data-bs-target', '#loginModal');
                memberButton.onclick = null;
                memberDropdown.style.display = 'none';
            }
            return data.isLoggedIn;
        } catch (error) {
            console.error('�ˬd�n�J���A�ɵo�Ϳ��~:', error);
            return false;
        }
    }

    // �B�z�n�J��洣��
    loginForm.addEventListener('submit', async function (event) {
        event.preventDefault();
        errorMessage.style.display = 'none';  // �M�����e�����~�T��

        // ������ƾ�
        const formData = new FormData(loginForm);

        try {
            const response = await fetch('/Account/Login', {
                method: 'POST',
                body: formData
            });

            if (response.ok) {
                // �n�J���\
                loginModal.hide();                // ���õn�J����
                await checkLoginStatus();         // ��s�n�J���A
                window.location.reload();         // ���s���J����
            } else {
                // �n�J����
                const text = await response.text();
                errorMessage.textContent = '�b���αK�X���~';
                errorMessage.style.display = 'block';
            }
        } catch (error) {
            console.error('�n�J�L�{�o�Ϳ��~:', error);
            errorMessage.textContent = '�n�J�L�{�o�Ϳ��~�A�еy��A��';
            errorMessage.style.display = 'block';
        }
    });

    // �K�X��ܤ����\��
    togglePasswordBtn.addEventListener('click', function () {
        const type = passwordInput.type === 'password' ? 'text' : 'password';
        passwordInput.type = type;
        // ��s�����ϥ�
        const icon = togglePasswordBtn.querySelector('i');
        icon.className = `bi bi-eye${type === 'password' ? '-slash' : ''}`;
    });

    // �I��������L�ϰ�������U�Կ��
    document.addEventListener('click', function (event) {
        if (!memberButton.contains(event.target) &&
            !memberDropdown.contains(event.target)) {
            memberDropdown.style.display = 'none';
        }
    });

    // �n�X�\��
    logoutButton.addEventListener('click', async function (event) {
        event.preventDefault();
        try {
            const response = await fetch('/Account/Logout', {
                method: 'POST'
            });
            if (response.ok) {
                window.location.reload();  // �n�X�᭫�s���J����
            }
        } catch (error) {
            console.error('�n�X�ɵo�Ϳ��~:', error);
        }
    });

    // �������J���ˬd�n�J���A
    checkLoginStatus();
});