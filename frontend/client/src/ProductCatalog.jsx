import React, { useState, useEffect } from 'react';
import { FixedSizeList as List } from 'react-window';
import './ProductCatalog.css';

const ProductCatalog = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const [debouncedSearchTerm, setDebouncedSearchTerm] = useState(searchTerm);
  
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const timerId = setTimeout(() => {
      setDebouncedSearchTerm(searchTerm);
    }, 500);

    return () => {
      clearTimeout(timerId);
    };
  }, [searchTerm]);

  useEffect(() => {
    const fetchProducts = async () => {
      if (!debouncedSearchTerm) {
        const data = await fetchProductsResponse()
        setProducts(data.products || []);
        setLoading(false);
        return;
      }
      setLoading(true);
      setError(null);
      try {
        const data = await fetchProductsResponse(debouncedSearchTerm);
        setProducts(data.products || []);
      } catch (e) {
        console.error("Failed to fetch products:", e);
        setError("Failed to load products. Check that the backend server is running and accessible.");
        setProducts([]);
      } finally {
        setLoading(false);
      }
    };

    fetchProducts();
  }, [debouncedSearchTerm]);

  const headers = [
    'Name', 'Description', 'Category', 'Brand', 'Price', 'Stock', 'SKU',
    'Release Date', 'Status', 'Rating', 'Colors', 'Sizes'
  ];

  const Row = ({ index, style }) => {
    const product = products[index];
    const formatArray = (arr) => (arr && arr.length > 0) ? arr.join(', ') : 'N/A';
    
    return (
      <div className="table-row" style={style}>
        <div title={product.name}>{product.name}</div>
        <div title={product.description}>{product.description}</div>
        <div>{product.category}</div>
        <div>{product.brand}</div>
        <div>${product.price.toFixed(2)}</div>
        <div>{product.stockQuantity}</div>
        <div>{product.sku}</div>
        <div>{new Date(product.releaseTimestampUtc).toLocaleDateString()}</div>
        <div>{product.availabilityStatus === 1 ? 'Available' : 'Unavailable'}</div>
        <div>{`${product.customerRating} / 5`} ⭐</div>
        <div>{formatArray(product.availableColors)}</div>
        <div>{formatArray(product.availableSizes)}</div>
      </div>
    );
  };

  return (
    <div className="catalog-container">
      <div className="search-container">
        <input
          type="text"
          className="search-input"
          placeholder="Search for products..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
      </div>

      <div className="product-table">
        <div className="table-header">
          {headers.map((header) => (
            <div key={header}><b>{header}</b></div>
          ))}
        </div>
        
        <div className="table-body">
          {loading && <div className="status-message">Loading... ⏳</div>}
          {error && <div className="status-message error">{error}</div>}
          {!loading && !error && products.length === 0 && (
            <div className="status-message">No products found.</div>
          )}
          {!loading && !error && products.length > 0 && (
            <List
              height={700}
              itemCount={products.length}
              itemSize={65}
              width="100%"
            >
              {Row}
            </List>
          )}
        </div>
      </div>
    </div>
  );
};

export default ProductCatalog;

async function fetchProductsResponse(searchTerm = null) {
  const url = new URL('https://localhost:7120/api/Products');

  if (searchTerm) {
    url.searchParams.append('searchTerm', searchTerm);
  }

  const response = await fetch(url);

  if (!response.ok) {
    throw new Error(`Network response was not ok: ${response.statusText}`);
  }

  return await response.json();
}
