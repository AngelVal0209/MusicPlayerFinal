describe('loginIncorrecto', () => {
  it('loginIncorrecto', () => {
    cy.visit('https://localhost:7083/');
    cy.get('#CorreoElectronico').click();
    cy.get('#CorreoElectronico').type('test@test.com');
    cy.get('#Contrasena').click();
    cy.get('#Contrasena').type('test1235');
    cy.get('.btn-login').click();
    cy.get('div.alert-danger').should('contain.text', 'Correo o contraseña incorrectos.');
  });
});