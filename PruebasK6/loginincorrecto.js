import http from 'k6/http';
import { check } from 'k6';

export default function () {
  const url = 'https://localhost:7083/Login';

  const payload = {
    CorreoElectronico: 'falso@correo.com',
    Contrasena: 'incorrecto',
  };

  const params = {
    headers: {
      'Content-Type': 'application/x-www-form-urlencoded',
    },
  };

  const res = http.post(url, payload, params);

  check(res, {
    'login incorrecto da status 200': (r) => r.status === 200,
    'muestra mensaje de error': (r) =>
      r.body.includes('Correo o contraseña incorrectos'),
  });
}
