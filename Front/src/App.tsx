import React, { useState, useEffect } from 'react';
import './App.css';
import axios from 'axios';
import { Table } from 'react-bootstrap';

const App = () => {

  const [data, setData] = useState([]);

  useEffect(() => {
    if (data.length == 0) {
      axios.get('http://localhost:4242/api/maestros')
        .then((data: any) => {
          console.log(data);
          setData(data.data);
        })
    }
  });

  return (
    <>
      <h1>Prueba React y .Net core en Docker</h1>

      {data ?
        <Table striped bordered hover>
          <thead>
            <tr>
              <th>#</th>
              <th>Nombre</th>
              <th>Activo</th>
            </tr>
          </thead>
          <tbody>
            {
              data.map((x: any) => {
                return (
                  <tr>
                    <td>{x.id}</td>
                    <td>{x.nombre}</td>
                    <td>{x.activo === true ? 'sí' : 'no'}</td>
                  </tr>)
              })
            }

          </tbody>
        </Table>
        : null}

    </>
  );
}

export default App;