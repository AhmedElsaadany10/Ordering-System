const API_BASE = 'https://localhost:8090/api/Auth';
let isRegister = false;

function toggleForm() {
  isRegister = !isRegister;
  document.getElementById('formTitle').textContent = isRegister ? 'Register' : 'Login';
  document.getElementById('submitBtn').textContent = isRegister ? 'Register' : 'Login';
  document.getElementById('toggleText').innerHTML = isRegister
    ? 'Already have an account? <a href="#" onclick="toggleForm()">Login</a>'
    : "Don't have an account? <a href=\"#\" onclick=\"toggleForm()\">Register</a>";

  document.getElementById('confirmPasswordField').classList.toggle('d-none', !isRegister);
}

function getFormData() {
  return {
    username: document.getElementById('username').value.trim(),
    password: document.getElementById('password').value.trim(),
    confirmPassword: document.getElementById('confirmPassword')?.value.trim()
  };
}

async function handleRegister(data) {
  if ( !data.username || !data.password || !data.confirmPassword) {
    return alert('Please fill in all fields');
  }
  if (data.password !== data.confirmPassword) {
    return alert('Passwords do not match');
  }

  try {
    const response = await fetch(`${API_BASE}/register`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        username: data.username,
        password: data.password
      })
    });

    const result = await response.json();
    toggleForm();
    alert(result.message || 'Registered successfully');
  } catch (error) {
    console.error('Registration error:', error);
    alert('Registration failed');
  }
}

async function handleLogin(data) {
  if (!data.username || !data.password) {
    return alert('Please enter username and password');
  }

  try {
    const response = await fetch(`${API_BASE}/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        username: data.username,
        password: data.password
      })
    });

    const result = await response.json();
    if (response.ok) {
      localStorage.setItem("userData",JSON.stringify(result));
      let  user =localStorage.getItem('userData')? JSON.parse(localStorage.getItem('userData')):{};
      if(user.role=='Admin')
        window.location.href="admin.html";
      else
      window.location.href="orders.html";
    } else {
      alert(result.message || 'Login failed');
    }
  } catch (error) {
    console.error('Login error:', error);
    alert('Login failed');
  }
}

document.getElementById('authForm').addEventListener('submit', function (e) {
  e.preventDefault();
  const data = getFormData();
  isRegister ? handleRegister(data) : handleLogin(data);
});