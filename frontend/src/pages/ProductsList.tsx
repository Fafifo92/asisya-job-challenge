import React, { useEffect, useState } from 'react';
import { DataGrid, GridColDef, GridPaginationModel, GridFilterModel } from '@mui/x-data-grid';
import { Container, Typography, TextField, Box, Button } from '@mui/material';
import api from '../utils/api';
import { useNavigate } from 'react-router-dom';

const ProductsList = () => {
  const [rows, setRows] = useState([]);
  const [totalRows, setTotalRows] = useState(0);
  const [paginationModel, setPaginationModel] = useState<GridPaginationModel>({
    page: 0,
    pageSize: 10,
  });
  const [searchTerm, setSearchTerm] = useState('');
  const navigate = useNavigate();

  const fetchProducts = async () => {
    try {
      const response = await api.get('/Product', {
        params: {
          PageNumber: paginationModel.page + 1,
          PageSize: paginationModel.pageSize,
          SearchTerm: searchTerm,
        },
      });
      setRows(response.data.items);
      setTotalRows(response.data.totalCount);
    } catch (error) {
      console.error("Error fetching products", error);
    }
  };

  useEffect(() => {
    fetchProducts();
  }, [paginationModel, searchTerm]);

  const columns: GridColDef[] = [
    { field: 'productID', headerName: 'ID', width: 70 },
    { field: 'productName', headerName: 'Nombre', width: 200 },
    { field: 'categoryName', headerName: 'Categoría', width: 130 },
    { field: 'unitPrice', headerName: 'Precio', width: 100, type: 'number' },
    { field: 'unitsInStock', headerName: 'Stock', width: 100, type: 'number' },
  ];

  return (
    <Container maxWidth="lg" sx={{ mt: 4 }}>
      <Typography variant="h4" gutterBottom>
        Listado de Productos
      </Typography>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
        <TextField
          label="Buscar Producto"
          variant="outlined"
          size="small"
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
        <Button variant="contained" color="primary" onClick={() => navigate('/products/new')}>
          Nuevo Producto
        </Button>
      </Box>
      <div style={{ height: 600, width: '100%' }}>
        <DataGrid
          rows={rows}
          columns={columns}
          rowCount={totalRows}
          paginationMode="server"
          paginationModel={paginationModel}
          onPaginationModelChange={setPaginationModel}
          pageSizeOptions={[10, 20, 50]}
          getRowId={(row) => row.productID}
        />
      </div>
    </Container>
  );
};

export default ProductsList;
