const url_server = `https://localhost:7037`
async function getToken() {
    const url_auth = `${url_server}/api/apiuser/Auth`
    return await fetch(
        url_auth,
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                email: 'mario@gmail.com',
                password: '0000'
            })
        }
    ).then(response => {
        if(!response.ok)
            throw new Error('Fail to fetch JWT Token ...')
        return response.json()
    }).then(data => {
        return data.token
    })
    .catch(err => console.log(err))
}
async function loadProducts() {
    const url_products = `${url_server}/api/apiproduct`
    const token = await getToken()
    return await fetch(
        url_products,
        {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        }
    ).then(response => {
        if(!response.ok)
            throw new Error('Fail to get products ...')
        return response.json()
    }).then(products => {
        let result = ''
        products.forEach(p => {
            result += 
            `
                <div class="card" style="width: 18rem;">
                    <img src="./img/product_test.jpg" class="card-img-top" alt="">
                    <div class="card-body">
                        <h5 id="titleId" class="card-title">${p.name}</h5>
                        <p id="descriptionId" class="card-text">${p.description}</p>
                        <p id="priceId" class="card-text">${p.price}</p>
                        <a href="#" class="btn btn-primary">buy</a>
                    </div>
                </div>
            `
        });
        document.getElementById("parent_products")
        .innerHTML = result
    })
}
loadProducts()
