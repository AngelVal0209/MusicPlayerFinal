describe('modificarUsuario', () => {
  it('modificarUsuario', () => {
    cy.visit('https://localhost:7083/');
    cy.get('#CorreoElectronico').click();
    cy.get('#CorreoElectronico').type('test@test.com');
    cy.get('form').click();
    cy.get('#Contrasena').click();
    cy.get('#Contrasena').type('test123');
    cy.get('.btn-login').click();
    cy.contains('a.fw-bold', 'Bienvenido, Test').click();
    cy.get('#NombreUsuario').click();
    cy.get('#NombreUsuario').type('testcambiado');
    cy.get('#CorreoElectronico').click();
    cy.get('#CorreoElectronico').click();
    cy.get('#CorreoElectronico').clear().type('nuevo@correo.com');
    cy.get('.fw-bold:nth-child(4)').click();
    cy.get('#ViejaContrase_a').click();
    cy.get('#ViejaContrase_a').type('test123');
    cy.get('#NuevaContrasena').click();
    cy.get('#NuevaContrasena').type('tester');
    cy.get('.text-center > .btn:nth-child(1)').click();
    cy.get('div.alert-success').should('contain.text', 'Se actualizó la información correctamente.');
  });
});