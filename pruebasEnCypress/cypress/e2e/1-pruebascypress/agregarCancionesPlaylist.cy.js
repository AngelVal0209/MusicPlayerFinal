describe('agregarCancionesPlaylist', () => {
  it('agregarCancionesPlaylist', () => {
    cy.visit('https://localhost:7083/');
    cy.get('#CorreoElectronico').click().type('test@test.com');
    cy.get('#Contrasena').click().type('test123');
    cy.get('.btn-login').click();

    cy.contains('Musica').click();
    cy.contains('Agregar Canción').click();
    cy.get('tr:nth-child(1) .btn').click();
    cy.contains('th', 'Título').should('exist');
  });
});
