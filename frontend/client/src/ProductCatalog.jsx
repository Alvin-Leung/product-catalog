import React, { useState, useEffect } from 'react';
import { FixedSizeList as List } from 'react-window';
import './ProductCatalog.css';

const ProductCatalog = () => {
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const response = await fetch('https://localhost:7120/api/Products');
        if (!response.ok) {
          throw new Error(`Network response was not ok: ${response.statusText}`);
        }
        const data = await response.json();
        setProducts(data.products || []);
      } catch (e) {
        console.error("Failed to fetch products:", e);
        setError("Failed to load products. Check that the backend server is running and accessible.");
      } finally {
        setLoading(false);
      }
    };

    fetchProducts();
  }, []);

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

  if (loading) {
    return <div className="status-message">Loading products... ⏳</div>;
  }

  if (error) {
    return <div className="status-message error">{error} ❌</div>;
  }

  return (
    <div className="product-table">
      <div className="table-header">
        {headers.map((header) => (
          <div key={header}><b>{header}</b></div>
        ))}
      </div>
      
      <div className="table-body">
        <List
          height={700}
          itemCount={products.length}
          itemSize={65}
          width="100%"
        >
          {Row}
        </List>
      </div>
    </div>
  );
};

export default ProductCatalog;