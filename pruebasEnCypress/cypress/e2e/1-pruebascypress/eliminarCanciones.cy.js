describe('eliminarCanciones', () => {
  it('eliminarCanciones', () => {
    cy.visit('https://localhost:7083/');
    cy.get('#CorreoElectronico').click();
    cy.get('#CorreoElectronico').click();
    cy.get('#CorreoElectronico').type('test@test.com');
    cy.get('#Contrasena').click();
    cy.get('#Contrasena').type('test123');
    cy.get('.btn-login').click();
    cy.contains('Musica').click();
    cy.on('window:confirm', () => true);
    cy.get('tr:nth-child(1) .btn').click();
    cy.on('window:confirm', () => true);
    cy.on('window:confirm', () => true);
    cy.get('p').should('contain.text', 'No hay canciones en esta playlist');

  });
});