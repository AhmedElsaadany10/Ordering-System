const API_BASE = 'https://localhost:8090/api';
const userData = localStorage.getItem('userData') ? JSON.parse(localStorage.getItem('userData')) : {};
let products = [];
  // Check if the user is admin
  function checkAdminRole() {
    if (userData.role !== 'Admin') {
      window.location.href = 'index.html'; // Or show a message
    } else {
      loadProducts(); // Fetch products if admin
      loadCustomers();
    }
  }

  // Fetch products from API
  async function loadProducts() {
    try {
      const productsRes = await fetch(`${API_BASE}/products`, {
        headers: {
          Authorization: `Bearer ${userData.token}`
        }
      });
      products = await productsRes.json();
      console.log(products);
      renderProducts(products);
    } catch (error) {
      console.error("Error fetching user or products", error);
    }
  }

  // Fetch Customers from API
  async function loadCustomers() {
    try {
      const response = await fetch(`${API_BASE}/admin/customers`, {
        headers: {
          Authorization: `Bearer ${userData.token}`
        }
      });
      const users = await response.json();
      console.log(users)
      renderCustomers(users);
    } catch (error) {
      console.error('Error fetching customers:', error);
    }
  }

  // Display products in the UI
  function renderProducts(products) {
    const productsContainer = document.getElementById('productsContainer');
    productsContainer.innerHTML = ''; // Clear existing content

    products.forEach((product,index) => {
      const productCard = `
      <tr id="product-${product.id}">
        <td>${index + 1}</td>
        <td>${product.name}</td>
        <td>${product.description}</td>
        <td>${product.price}$</td>
        <td>
          <button class="btn btn-sm btn-warning" onclick="editProduct(${product.id})">Edit</button>
          <button class="btn btn-sm btn-danger" onclick="deleteProduct(${product.id})">Delete</button>
          <button class="btn btn-sm btn-primary" onclick="loadProductCustomers(${product.id})">Details</button>
        </td>
        </tr>
    `;
      productsContainer.innerHTML += productCard;
    });
  }

  // Display Customers in the UI
  function renderCustomers(customers) {
    const customersTable = document.getElementById('usersTable');
    customersTable.innerHTML = ''; // Clear existing content

    customers.forEach((customer,index) => {
      const userRow = `
        <tr>
          <td>${index+1}</td>
          <td>${customer.username}</td>
          <td>${customer.bannedUntil?'Banned':'Active'}</td>
          <td>${customer.bannedUntil}</td>
          <td><button class="btn btn-primary" onclick="loadCustomerOrder('${customer.id}')">View products</button></td>
        </tr>
      `;
      customersTable.innerHTML += userRow;
    });
  }

/////Create Products
async function createProduct() {
  const name = document.getElementById("productName").value;
  const description = document.getElementById("productDescription").value;
  const price = parseFloat(document.getElementById("productPrice").value); 

  if (!name || !description || isNaN(price)) {
    alert("Please fill in all fields correctly.");
    return;
  }

  const res = await fetch(`${API_BASE}/products`, {
    method: 'POST',
    headers: {
      'Authorization': `Bearer ${userData.token}`,
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ name, description, price }),
  })
    .then(async res => {
      if (!res.ok) {
        console.log(res);
        const data = await res.json();
        console.error("Error:", data);
        alert(data.error || "Something went wrong");
      } else {
        alert("Product created successfully!");
        const newproduct = await res.json();
        products.push(newproduct);
        renderProducts(products);
        document.getElementById('btn-close').click();

        //Clear the form fields after successful addition
        document.getElementById("productName").value = "";
        document.getElementById("productDescription").value = "";
        document.getElementById("productPrice").value = "";
      }
    })
    .catch(error => {
      console.error("Fetch failed:", error);
    });
}

/////Edit Products
function editProduct(id) {
  currentEditingproduct = products.find(product => product.id === id);

  // Pre-fill the modal fields with the current product data
  document.getElementById("editProductName").value = currentEditingproduct.name;
  document.getElementById("editProductDescription").value = currentEditingproduct.description;
  document.getElementById("editProductPrice").value = currentEditingproduct.price;
  // Open the modal
  new bootstrap.Modal(document.getElementById('editProductModal')).show();
}

// Update product in Api
async function updateProduct() {
  const name = document.getElementById("editProductName").value;
  const description = document.getElementById("editProductDescription").value;
  const price = parseFloat(document.getElementById("editProductPrice").value);

  const res = await fetch(`${API_BASE}/products/${currentEditingproduct.id}`, {
    method: 'PUT',
    headers: {
      'Authorization': `Bearer ${userData.token}`,
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ name, description,price }),
  });

  if (res.ok) {
    alert("product updated successfully!");

    // Update the product in the products array and re-render the UI
    currentEditingproduct.name = name;
    currentEditingproduct.description = description;
    currentEditingproduct.price = price;
    document.getElementById('btn-close-edit').click()// Close modal
    renderProducts(products);
  } else {
    const data = await res.json();
    user.isBan=true;
    alert(data.error); 
  }
}

//////delete product
async function deleteProduct(id) {
  if (confirm("Are you sure you want to delete this product?")) {
    try {
      const res = await fetch(`${API_BASE}/products/${id}`, {
        method: 'DELETE',
        headers: {
          Authorization: `Bearer ${userData.token}`
        }
      });

      if (res.ok) {
        alert("product deleted successfully!");

        // Remove the product from the array and UI directly
        products = products.filter(product => product.id !== id);
        document.getElementById(`product-${id}`).remove(); // Remove from DOM
      } else {
        const data = await res.json();
        alert(data.error); 
      }
    } catch (error) {
      console.error("Error deleting product", error);
      alert("An error occurred while deleting the product.");
    }
  }
}


 // Fetch Customer Orders from API
 async function loadCustomerOrder(id) {
  try {
    const productsRes = await fetch(`${API_BASE}/admin/orders-customer/${id}`, {
      headers: {
        Authorization: `Bearer ${userData.token}`
      }
    });
    orders = await productsRes.json();
    console.log(orders);
    showCustomerorderesModal(orders)
  } catch (error) {
    console.error("Error fetching user or products", error);
  }
}

    // Show Customer Orders in the modal
    function showCustomerorderesModal(orders) {
        const customerOrdersContainer = document.getElementById('userOrdersContainer');
        customerOrdersContainer.innerHTML = ''; // Clear existing content
  
        orders.forEach((order, index) => {
          const productCard = `
          <th scope="row">${index + 1}</th>
          <td>${order.name}</td>
          <td>${order.price}$</td>
          <td>${order.createdAt}</td>
        `;
          customerOrdersContainer.innerHTML += productCard;
        });
  
        // Show the modal with user products
        new bootstrap.Modal(document.getElementById('userOrdersModal')).show();
      }

 // Fetch Customer Orders from API
 async function loadProductCustomers(id) {
  try {
    const productsRes = await fetch(`${API_BASE}/products/${id}`, {
      headers: {
        Authorization: `Bearer ${userData.token}`
      }
    });
    orders = await productsRes.json();
    console.log(orders);
    showProductCustomersModal(orders)
  } catch (error) {
    console.error("Error fetching customers or products", error);
  }
}
    // Show Product Customers in the modal
          function showProductCustomersModal(orders) {
        const customerOrdersContainer = document.getElementById('productCustomersContainer');
        customerOrdersContainer.innerHTML = ''; // Clear existing content
  
        orders.forEach((order, index) => {
          const productCard = `
          <th scope="row">${index + 1}</th>
          <td>${order.customerName}</td>
          <td>${order.createdAt}</td>
        `;
          customerOrdersContainer.innerHTML += productCard;
        });
  
        // Show the modal with user products
        new bootstrap.Modal(document.getElementById('productCustomersModal')).show();
      }

  // Logout function
  function logout() {
    localStorage.removeItem('userData'); // Remove user data from localStorage
    window.location.href = 'index.html'; // Redirect to login page
  }

  // Attach the logout function to the logout button
  document.getElementById('logoutButton').addEventListener('click', logout);

  // Initialize on page load
document.addEventListener('DOMContentLoaded', checkAdminRole);