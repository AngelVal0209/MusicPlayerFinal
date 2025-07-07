import http from 'k6/http';
import { check } from 'k6';

export default function () {
  const url = 'https://localhost:7083/Login';

  const payload = {
    CorreoElectronico: 'test@test.com',
    Contrasena: 'tester',
  };

  const params = {
    headers: {
      'Content-Type': 'application/x-www-form-urlencoded',
    },
    redirects: 0, 
  };

  const res = http.post(url, payload, params);

  check(res, {
    'login fue redirigido (302)': (r) => r.status === 302,
  });
}
