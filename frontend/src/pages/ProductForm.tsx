import React from 'react';
import { useForm } from 'react-hook-form';
import { TextField, Button, Box, Typography, Container, Paper } from '@mui/material';
import api from '../utils/api';
import { useNavigate } from 'react-router-dom';

const ProductForm = () => {
  const { register, handleSubmit } = useForm();
  const navigate = useNavigate();

  const onSubmit = async (data: any) => {
    try {
      // Convert numeric fields
      const payload = {
        ...data,
        unitPrice: parseFloat(data.unitPrice),
        unitsInStock: parseInt(data.unitsInStock),
        categoryID: parseInt(data.categoryID) || null
      };
      await api.post('/Product', payload);
      navigate('/products');
    } catch (error) {
      console.error("Error creating product", error);
      alert("Error al crear producto");
    }
  };

  return (
    <Container component="main" maxWidth="sm">
      <Paper elevation={3} sx={{ padding: 4, marginTop: 4 }}>
        <Typography variant="h5" gutterBottom>
          Nuevo Producto
        </Typography>
        <Box component="form" onSubmit={handleSubmit(onSubmit)} sx={{ mt: 1 }}>
          <TextField
            margin="normal"
            required
            fullWidth
            label="Nombre del Producto"
            {...register('productName')}
          />
          <TextField
            margin="normal"
            fullWidth
            label="ID de Categoría (Opcional)"
            type="number"
            {...register('categoryID')}
          />
           <TextField
            margin="normal"
            fullWidth
            label="Precio Unitario"
            type="number"
            inputProps={{ step: "0.01" }}
            {...register('unitPrice')}
          />
           <TextField
            margin="normal"
            fullWidth
            label="Unidades en Stock"
            type="number"
            {...register('unitsInStock')}
          />
          <Button
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
          >
            Guardar
          </Button>
          <Button
            fullWidth
            variant="outlined"
            onClick={() => navigate('/products')}
          >
            Cancelar
          </Button>
        </Box>
      </Paper>
    </Container>
  );
};

export default ProductForm;
