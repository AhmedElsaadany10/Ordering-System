const API_BASE = 'https://localhost:8090/api';
let products = [];
let orders = [];
let user =localStorage.getItem('userData')? JSON.parse(localStorage.getItem('userData')):{};
let currentEditingproduct = null;
function isAuthenticated() {
  return !!localStorage.getItem('userData');
}

function logout() {
  localStorage.removeItem('userData');
  window.location.href = 'index.html';
}

function checkAccess() {
   if (!isAuthenticated()) {
    alert('You must be logged in to view this page.');
     window.location.href = 'index.html';
   }
}

function getUserData(user) {
  const usernameDisplay = document.getElementById("usernameDisplay");
 renderBan(user.isBan);
  const userInfo = `${user.username}`;
  usernameDisplay.innerHTML += userInfo;
}
//////set ban tab
function renderBan(isBan){
  const container = document.getElementById("isBanContainer");
  container.innerHTML="";
  container.innerHTML=isBan?
  `<span class="badge bg-danger">Banned</span>` :
  `<span class="badge bg-success">Active</span>`;
}

//*************  Get Products  ******************** */
/////load products from Api
async function loadProducts() {
  try {
    const productsRes = await fetch(`${API_BASE}/products`, {
      headers: {
        Authorization: `Bearer ${user.token}`
      }
    });
    products = await productsRes.json();
    renderProducts(products);
  } catch (error) {
    console.error("Error fetching user or products", error);
  }
}

/////render products
function renderProducts(products) {
  const container = document.getElementById("productsContainer");
  container.innerHTML = "";
  // Render products
  products.forEach(product => {
    container.innerHTML += `
          <div class="col-md-6 col-lg-3 mt-4" id="product-${product.id}">
            <div class="card shadow-sm h-100">
              <div class="card-body d-flex flex-column">
                <h5 class="card-title">${product.name}</h5>
                <p class="card-text">${product.description}</p>
                <p class="card-text"> Price :${product.price}$</p>
                <div class="mt-auto d-flex justify-content-between">
                  <button class="btn btn-sm btn-primary ms-auto" ${user.isBan?'disabled':''} onclick="createOrder(${product.id})">Add</button>
                </div>
              </div>
            </div>
          </div>
        `;
  });
}


//*************  Get Orders  ******************** */

/////load orders from Api
async function loadOrders() {
  try {
    const productsRes = await fetch(`${API_BASE}/orders`, {
      headers: {
        Authorization: `Bearer ${user.token}`
      }
    });
    orders = await productsRes.json();
    renderOrders(orders);
  } catch (error) {
    console.error("Error fetching user or products", error);
  }
}
/////render Orders
function renderOrders(orders) {
  const container = document.getElementById("ordersContainer");
  container.innerHTML = "";
  // Render Orders
  orders.forEach(order => {
    container.innerHTML += `
          <div class="col-md-6 col-lg-3 mt-4" id="order-${order.orderId}">
            <div class="card shadow-sm h-100">
              <div class="card-body d-flex flex-column">
                <h5 class="card-title">${order.productName}</h5>
                <p class="card-text"> Price : ${order.price}$</p>
                <div class="mt-auto d-flex justify-content-between">
                  <button class="btn btn-sm btn-danger" onclick="deleteOrder(${order.orderId})">Delete</button>
                  <button class="btn btn-sm btn-primary " onclick="showOrderDetails(${order.orderId})">Details</button>
                </div>
              </div>
            </div>
          </div>
        `;
  });
}

//*************  Add Orders  ******************** */

/////create Order
async function createOrder(productId) {
  const res = await fetch(`${API_BASE}/orders`, {
    method: 'POST',
    headers: {
      'Authorization': `Bearer ${user.token}`,
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ customerId:user.id, productId }),
  }).then(async res => {
    if (!res.ok) {
        const data = await res.json();
        console.error("Error:", data); 
        alert(data.message); 
    } else {
        alert("product created successfully!");

        // Add the new product directly to the UI (no need to fetch again)
        const newOrder = await res.json();
        orders.push(newOrder); // Update the products array
        renderOrders(orders); // Re-render with the new product
        document.querySelector('.btn-close').click(); // Close modal
    }
})
.catch(error => {
    console.error("Fetch failed:", error);
});

}

//*************  Delete Orders  ******************** */
//delete order
async function deleteOrder(id) {
  if (confirm("Are you sure you want to delete this Order?")) {
    try {
      const res = await fetch(`${API_BASE}/orders/${id}`, {
        method: 'DELETE',
        headers: {
          Authorization: `Bearer ${user.token}`
        }
      });

      if (res.ok) {
        const data = await res.json();
        alert(data.message); 
        if(data.isBan){
          user.isBan=true;
          localStorage.setItem('userData',JSON.stringify(user))
          renderBan(user.isBan);
          renderProducts(products)
        }
        orders = orders.filter(order => order.orderId !== id);
        document.getElementById(`order-${id}`).remove(); // Remove from DOM
      } else {
        const data = await res.json();
        alert(data.message); 
      }
    } catch (error) {
      console.error("Error deleting order", error);
      alert("An error occurred while deleting the order.");
    }
  }
}

    // Show product details in the modal

    function showOrderDetails(orderId) {
      const order = orders.find(order => order.orderId == orderId);
      const orderContainer = document.getElementById('orderDetailsContainer');
      if(order){
      orderContainer.innerHTML = `
            <h5 class="card-title mb-3">${order.productName}</h5>
            <p class="card-text">${order.description}</p>
            <p class="card-text">Price : ${order.price}$</p>
            <p class="card-text">Created At :  ${order.createdAt}</p>
    `; }

      // Show the modal with user products
      new bootstrap.Modal(document.getElementById('orderDetailsModal')).show();
    }

// Initialize page
checkAccess();
getUserData(user);
loadProducts();
loadOrders();




