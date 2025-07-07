import http from 'k6/http';
import { check } from 'k6';

export default function () {
  const url = 'https://localhost:7083/Registro';

  const timestamp = Date.now();
  const correo = `usuario${timestamp}@mail.com`;

  const payload = {
    NombreUsuario: `k6usuario${timestamp}`,
    CorreoElectronico: correo,
    Contrasena: '1234',
    ConfirmarContraseña: '1234',
    FechaNacimiento: '2000-01-01',
    Genero: 'Masculino',
  };

  const params = {
    headers: {
      'Content-Type': 'application/x-www-form-urlencoded',
    },
    redirects: 0,
  };

  const res = http.post(url, payload, params);

  check(res, {
    'registro redirige al login (302)': (r) => r.status === 302,
  });
}
