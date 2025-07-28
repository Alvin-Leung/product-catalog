import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import ProductCatalog from './ProductCatalog.jsx'

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <ProductCatalog />
  </StrictMode>,
)
