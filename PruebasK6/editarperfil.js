import http from 'k6/http';
import { check } from 'k6';

export default function () {
  const cookie = '.AspNetCore.Cookies=CfDJ8Nyo-GRdgXJMj8HN4XlUxZ80mGg0ymvCdasAWqYGnvyF9fFIIyJLJl8cbt_g_92iuKmnfxzaVsbOSQuEztzjOlboxyh06fCDA826Us8jZflhf3zvmOgy_PpEztFihtmA6D28zvxvhlNrLNiDPxJkZKbfXxoHcc8iIYie4fwcbko0Ig3IoUyyQnddNqQSrX88MMdIAkgJQbBp7NhnJ0t6fZLuU8CrjIMtAYzkuH7vU4DexnhBn7pKsZoVeqB1KcNvXZ67dwcOhaQv6bpph-2PESBnCyIXYON3APfDu-ws9_zqcbmYaBiZV43sSZrqU55bkYUQlhvcc0Jjezo01DCNxUOc8Vm68qSlpJoAJUtC59sdWzugaIWELXtocmzudjJZS5iBRM6FwxVzzMWgWSIzpywXhDYAZ4zFWDQa4QrlYmmFWUGd1R1kkXS7S0n1MVJWPg'; 

  const url = 'https://localhost:7083/EditarPerfil';

  const payload = {
    NombreUsuario: 'UsuarioEditadoK6',
    CorreoElectronico: 'nuevo_correo_k6@mail.com',
    ViejaContraseña: 'tester',        
    NuevaContrasena: 'tester',        
  };

  const params = {
    headers: {
      'Content-Type': 'application/x-www-form-urlencoded',
      'Cookie': cookie,
    },
  };

  const res = http.post(url, payload, params);

  check(res, {
    'perfil editado con éxito': (r) =>
      r.status === 200 || r.status === 302 || r.body.includes('Se actualizó la información'),
  });
}
