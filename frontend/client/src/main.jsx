import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import ProductCatalog from './ProductCatalog.jsx'

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <ProductCatalog />
  </StrictMode>,
)
