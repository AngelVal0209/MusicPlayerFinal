describe('loginCorrecto', () => {
  it('loginCorrecto', () => {
    cy.visit('https://localhost:7083/');
    cy.get('#CorreoElectronico').click();
    cy.get('#CorreoElectronico').type('test@test.com');
    cy.get('#Contrasena').click();
    cy.get('#Contrasena').type('test123');
    cy.get('.btn-login').click();
    cy.get('#mensajeBienvenida').should('contain.text', 'Qué bueno volverte a ver.');
  });
});